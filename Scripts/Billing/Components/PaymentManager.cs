//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------
#if UNITY_PURCHASING

using System;
using GameFramework.GameObjects.Components;
using GameFramework.Localisation;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Purchasing;

namespace GameFramework.Billing.Components
{

    /// <summary>
    /// Provides code for setting up and callind in app billing. This derives from IStoreListener to enable it to receive 
    /// messages from Unity Purchasing.
    /// </summary>
    [AddComponentMenu("Game Framework/Billing/PaymentManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class PaymentManager : SingletonPersistant<PaymentManager>, IStoreListener
    {
        [Header("Payment Setup")]

        /// <summary>
        /// Whether to initialise the payment backend on awake. 
        /// </summary>
        /// If you set this to false then be sure to manually call InitializePurchasing
        [Tooltip("Whether to initialise the payment backend on awake. \n\nIf you set this to false then be sure to manually call InitializePurchasing.")]
        public bool InitOnAwake = true;

        /// <summary>
        /// A list of the product id's that you use in your game. 
        /// </summary>
        /// These can either the build in product id's or your own custom ones. These should be the same as in the backend store.
        [Tooltip("A list of the product id's that you use in your game. THese can either the build in product id's or your own custom ones.\n\nThese should be the same as in the backend store.")]
        public PaymentProduct[] Products;

        // setup references
        IStoreController _controller;              // Reference to the Purchasing system.
        IExtensionProvider _extensions;            // Reference to store-specific Purchasing subsystems.

        /// <summary>
        /// Called on startup.
        /// </summary>
        protected override void GameSetup()
        {
            // Initialise purchasing
            if (InitOnAwake)
            {
                InitializePurchasing();
            }
        }

        /// <summary>
        /// Method to initialise the payment backend
        /// </summary>
        /// This is called automatically if you enable InitOnAwake, otherwise you will need to call it yourself
        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add products to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
            Assert.IsTrue(Products.Length > 0, "You need to add products if using Payments");
            foreach (PaymentProduct product in Products)
                builder.AddProduct(product.Name, product.ProductType);

            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// Returns whether the payment system is initialised and ready for use.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return _controller != null && _extensions != null;
        }


        /// <summary>
        /// Try and purchase the given product id.
        /// </summary>
        /// <param name="productId"></param>
        public virtual void BuyProductId(string productId)
        {
            // If the stores throw an unexpected exception, use try..catch to protect my logic here.
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {
                    // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                    Product product = _controller.products.WithID(productId);

                    // If the look up found a product for this device's store and that product is ready to be sold ... 
                    if (product != null && product.availableToPurchase)
                    {
                        Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        _controller.InitiatePurchase(product);
                    }
                    // Otherwise ...
                    else
                    {
                        // ... report the product look-up failure situation  
                        DialogManager.Instance.ShowError(textKey: "Billing.NotAvailable");
                    }
                }
                // Otherwise ...
                else
                {
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                    DialogManager.Instance.ShowError(textKey: "Billing.NotInitialised");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                // ... by reporting any unexpected exception for later diagnosis.
                DialogManager.Instance.ShowError(LocaliseText.Format("GeneralMessage.Error.GeneralError", e.ToString()));
            }
        }


        /// <summary>
        /// Restore purchases previously made by this customer. 
        /// </summary>
        /// Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
        public void RestorePurchases()
        {
            // If Purchasing has been initialised ...
            if (IsInitialized())
            {
                //TODO: the below conditional should not be needed as interfaces should return empty on unsupported platforms!
                // If we are running on an Apple device ... 
                //if (Application.platform == RuntimePlatform.IPhonePlayer || 
                //	Application.platform == RuntimePlatform.OSXPlayer)
                //{
                //	// ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = _extensions.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) => {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                    if (result)
                    {
                        // This does not mean anything was restored,
                        // merely that the restoration process succeeded.
                        DialogManager.Instance.ShowInfo(textKey: "Billing.RestoreSucceeded");
                    }
                    else {
                        // Restoration failed.
                        DialogManager.Instance.ShowError(textKey: "Billing.RestoreFailed");
                    }
                });
                //}
                //// Otherwise ...
                //else
                //{
                //	// We are not running on an Apple device. No work is necessary to restore purchases.
                //	Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
                //}
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                DialogManager.Instance.ShowError(textKey: "Billing.NotInitialised");
            }
        }


        /// <summary>
        /// Called when a purchase completes. This automatically handles certain types of purchase and notifications
        /// </summary>
        /// If you need custom processing then you may subclass PaymentManager and override this method.
        /// This may be called at any time after OnInitialized().
        public virtual PurchaseProcessingResult ProcessPurchase(string productId)
        {
            Payment.ProcessPurchase(productId);
            return PurchaseProcessingResult.Complete;
        }


        //  
        // --- IStoreListener
        //

        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            this._controller = controller;
            // Store specific subsystem, for accessing device-specific store features.
            this._extensions = extensions;
        }


        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        ///
        /// Note that this will not be called if Internet is unavailable; Unity IAP
        /// will attempt initialization until it becomes available.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            return ProcessPurchase(args.purchasedProduct.definition.id);
        }



        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            switch (failureReason)
            {
                // for these cases we don't need to inform further
                case PurchaseFailureReason.UserCancelled:
                    break;
                // for these we show an error
                default:
                    DialogManager.Instance.ShowError(LocaliseText.Format("GeneralMessage.Error.GeneralError", failureReason));
                    break;
            }
        }


        public override string ToString()
        {
            string result = "";
            if (_controller != null && _controller.products != null)
                foreach (var product in _controller.products.all)
                {
                    result += string.Format("{0}, {1}, {2}\n", product.metadata.localizedTitle, product.metadata.localizedDescription, product.metadata.localizedPriceString);
                }
            return result;
        }
    }

    [Serializable]
    public class PaymentProduct
    {
        public ProductType ProductType;
        public string Name;
    }
}

#endif
