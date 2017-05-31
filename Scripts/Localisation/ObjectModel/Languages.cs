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

using System.Collections.Generic;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// A list of supported languages and functionality for working with them
    /// </summary>
    public class Languages
    {
        /// <summary>
        /// Array of available language definitions
        /// </summary>
        public static readonly LanguageDefinition[] LanguageDefinitions =
        {
            new LanguageDefinition {Name = "Afrikaans",     Code = "af"},
            new LanguageDefinition {Name = "Arabic",        Code = "ar"},
            new LanguageDefinition {Name = "Basque",        Code = "eu"},
            new LanguageDefinition {Name = "Belarusian",    Code = "be"},
            new LanguageDefinition {Name = "Bulgarian",     Code = "bg"},
            new LanguageDefinition {Name = "Catalan",       Code = "ca"},
            new LanguageDefinition {Name = "Chinese(Simplified)",       Code = "zh-CN"},
            new LanguageDefinition {Name = "Chinese(Traditional)",      Code = "zh-TW"},
            new LanguageDefinition {Name = "Czech",         Code = "cs"},
            new LanguageDefinition {Name = "Danish",        Code = "da"},
            new LanguageDefinition {Name = "Dutch",         Code = "nl"},
            new LanguageDefinition {Name = "English",       Code = "en"},
            new LanguageDefinition {Name = "Estonian",      Code = "et"},
            new LanguageDefinition {Name = "Finnish",       Code = "fi"},
            new LanguageDefinition {Name = "French",        Code = "fr"},
            new LanguageDefinition {Name = "German",        Code = "de"},
            new LanguageDefinition {Name = "Greek",         Code = "el"},
            new LanguageDefinition {Name = "Hebrew",        Code = "iw"},
            new LanguageDefinition {Name = "Hungarian",     Code = "hu"},
            new LanguageDefinition {Name = "Icelandic",     Code = "is"},
            new LanguageDefinition {Name = "Indonesian",    Code = "id"},
            new LanguageDefinition {Name = "Italian",       Code = "it"},
            new LanguageDefinition {Name = "Japanese",      Code = "ja"},
            new LanguageDefinition {Name = "Korean",        Code = "ko"},
            new LanguageDefinition {Name = "Latvian",       Code = "lv"},
            new LanguageDefinition {Name = "Lithuanian",    Code = "lt"},
            new LanguageDefinition {Name = "Malay",         Code = "ms"},
            new LanguageDefinition {Name = "Norwegian",     Code = "no"} ,
            new LanguageDefinition {Name = "Polish",        Code = "pl"} ,
            new LanguageDefinition {Name = "Portuguese",    Code = "pt"} ,
            new LanguageDefinition {Name = "Romanian",      Code = "ro"} ,
            new LanguageDefinition {Name = "Russian",       Code = "ru"} ,
            new LanguageDefinition {Name = "Serbian",       Code = "sr"} ,
            new LanguageDefinition {Name = "Slovak",        Code = "sk"} ,
            new LanguageDefinition {Name = "Slovenian",     Code = "sl"} ,
            new LanguageDefinition {Name = "Spanish",       Code = "es"} ,
            new LanguageDefinition {Name = "Swedish",       Code = "sv"} ,
            new LanguageDefinition {Name = "Thai",          Code = "th"} ,
            new LanguageDefinition {Name = "Turkish",       Code = "tr"} ,
            new LanguageDefinition {Name = "Ukrainian",     Code = "uk"} ,
            new LanguageDefinition {Name = "Vietnamese",    Code = "vi"} 
        };

        /// <summary>
        /// Dictionary of languages for quick access.
        /// </summary>
        public static Dictionary<string, LanguageDefinition> LanguageDefinitionsDictionary;

        static Languages()
        {
            LanguageDefinitionsDictionary = new Dictionary<string, LanguageDefinition>();
            foreach (var languageDefinition in LanguageDefinitions)
                LanguageDefinitionsDictionary.Add(languageDefinition.Name, languageDefinition);
        }

        /// <summary>
        /// Get all language codes
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLanguageCodes()
        {
            var languageCodes = new List<string>();
            foreach (var languageDefinition in LanguageDefinitions)
                languageCodes.Add(languageDefinition.Code);
            return languageCodes;
        }
    }

    public class LanguageDefinition
    {
        public string Name;
        public string Code;
    }
}