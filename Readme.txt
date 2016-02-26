Game Framework - Free v1.0

Thank you for using Game Framework. 

This package features the main core of Game Framework. There is also a paid version which contains additional content to 
get you up and running including:

	- 3 UI themes (Cartoon, Space, RPG)
	- The full Beautiful Transitions asset
	- Additional 3D models
	- Additional samples
	- Tutorials
	- Other content
	- Priority support and feature requests
	- Secures future development of this framework
	
Feel free to try this version, however if you like the framework then please consider the small price of purchasing the 
paid version to support our efforts in developing this framework further.

Our goal is to create something that you enjoy and want to use. If you have any thoughts, comments, suggestions or 
otherwise then please contact us through our website or drop me a mail directly on mark_a_hewitt@yahoo.co.uk

Please consider rating this asset on the asset store.

Regards,
Mark Hewitt

For more details please visit: http://www.flipwebapps.com/game-framework/ 
For tutorials visit: http://www.flipwebapps.com/game-framework/tutorials/

- - - - - - - - - -

QUICK START

	1. If you have an older version installed:
		1.1. Make a backup of your project
		1.2. Delete the old /FlipWebApps folder to cater for possible conflicts.
	2. Select "Edit->Project Settings->Script Execution Order…".
	3. Drag the following scripts across and give them the specified priority:
		- FlipWebApps/GameFramework/Scripts/UI/Dialogs/Components/DialogInstance.cs (priority -100)
		- FlipWebApps/GameFramework/Scripts/GameStructure/GameManager.cs (priority -50)
		- FlipWebApps/GameFramework/Scripts/FreePrize/Components/FreePrizeManager.cs (priority -20)
	4. Setup and run the basic tutorial:
		4.1. Browse to "FlipWebApps/GameFramework"
		4.2. Open build settings from "Top Menu->File->Build Settings".
		4.3. Drag Demo-Game and Demo-Menu to "Scenes In Build" in the "Build Settings" window.
		4.4. Run the Demo-Menu scene.
		4.5. This is only a very basic demo so check the getting started tutorial at 
		     http://www.flipwebapps.com/game-framework/tutorials/

For full setup instructions visit: http://www.flipwebapps.com/game-framework/setup/		
		
- - - - - - - - - -

CHANGE LOG

v1.0
	Game Framework Core - Improvements
	- Deprecated legacy UI animations and added support for Beautiful Transitions asset (included with paid version)
	- Basic Demo scene added
	- Animation: SetTriggerOnce and SetBoolOnce components for running animations one time
	- Audio: Effect audio effect pooling and simultaneous effect support.
	- GameObjects: Generic EnableOnce component

	Game Framework Core - Fixes
	- Free Prize: Time to Free Prize subscribes to localisation changes

	Game Framework Extras - Improvements
	- UI / Themes: Updated all dialogs to use the new Beautiful Transitions asset.
	- Themes: Basic RPG theme.
	- Themes: Start of basic themes window to help with rebranding.

	Game Framework Extras - Fixes
	
	Game Framework Tutorials - Improvements
	- Advanced 'Extending Level' tutorial added
	- Updated all tutorials to use the new Beautiful Transitions asset.

	Game Framework Tutorials - Fixes

v0.9
	Game Framework Core - Improvements
	- IAP: Component to show / hide gameobjects depending upon whether IAP is enabled.
	- IAP: Support for full game unlock and unlocking of worlds and characters.
	- UI: Dynamic layout support for game over and settings windows.
	- UI: Support for chosing what is shown in game over and settings windows.
	- UI: Sync of button state changes to child text and image components for simpler and improved UI designs
	- Localisation: Language change notification.
	- Localisation: Dynamic settings option for choosing language.
	- Localisation: Button for choosing localisation language.
	- Localisation: Menu option for creating localisation file.
	- Localisation: Norwegian translation added.
	- Game Structure: Manual / automatic world select screen functionality.
	- GameObjects: RunOnState periodic frequency option.
	- GameObjects: GetPath() method.

	Game Framework Core - Fixes
	- Localisation: Fix for missing last character in certain conditions
	- UI: Button components refactored into new location

	Game Framework Extras - Improvements
	- UI: Updated all theme dialogs for dynamic layout and options.
	- Themes: Basic RPG theme.
	- Themes: Start of basic themes window to help with rebranding.

	Game Framework Extras - Fixes
	- UI: Removed old unlocklevel prefab content
	
	Game Framework Tutorials - Improvements
	- Dialog and Localisation tutorials

	Game Framework Tutorials - Fixes
	- Various fixes to several tutorials and bumped to newest prefabs.

v0.8.5
	Game Framework Core - Improvements
	- Animated score prefab
	- Support for animated dialog content
	- Support and enhancements for GameItem unlocking
	- Added free prize buttons to cheat window.
	- Score change animation support
	- GameObjectHelper GetParentNamedGameObject()

	Game Framework Core - Fixes
	- Fixed for incorrect conditional includes
	
	Game Framework Extras - Improvements
	- Added space theme and tutorial
	- New free prize 3D models and tutorial
	- Support for animated dialog content
	- Animated unlock functionality

	Game Framework Extras - Fixes
	- Fixes in several tutorial samples

v0.8
	First public release