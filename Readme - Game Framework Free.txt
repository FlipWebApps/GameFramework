Game Framework Free v0.9

Thank you for using Game Framework. 

This package features the main core of Game Framework. There is also a paid version which contains additional content to 
get you up and running including:

	- 3 UI themes (Cartoon, Space, RPG)
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
		1.2. Delete the old /FlipWebApps folder to cater for possible.
	2. Select "Edit->Project Settings->Script Execution Order…".
	3. Drag the following scripts across and give them the specified priority:
		- FlipWebApps/GameFramework/Scripts/UI/Dialogs/Components/DialogInstance.cs (priority -100)
		- FlipWebApps/GameFramework/Scripts/GameStructure/GameManager.cs (priority -50)
		- FlipWebApps/GameFramework/Scripts/FreePrize/Components/FreePrizeManager.cs (priority -20)
	4. If you have the paid version and want to run a tutorial:
		4.1. Browse to the tutorial you want to run e.g. "FlipWebApps/GameFrameworkTutorials/GettingStarted/Part5"
		4.2. Open build settings from "Top Menu->File->Build Settings".
		4.3. Drag all scenes in the tutorial folder to "Scenes In Build" in the "Build Settings" window.
		4.4. Play the scene (if there are multiple scenes then the one ending "Title" is typically the entry point)

For full setup instructions visit: http://www.flipwebapps.com/game-framework/setup/		
		
- - - - - - - - - -

CHANGE LOG

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