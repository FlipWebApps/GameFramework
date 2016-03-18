Game Framework - Free v1.3

Thank you for using Game Framework. 

This package features the main core of Game Framework. There is also a paid version which contains additional content to 
get you up and running including:

	- 3 UI themes (Cartoon, Space, RPG)
	- The full Beautiful Transitions asset for beautiful screen and UI wipes and transitions
	- Full 2D runner framework and game sample.
	- Advanced parallex scrolling script.
	- Additional 3D models and animations including for free prize
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
		1.1. Make a backup of your project.
		1.2. Delete the old /FlipWebApps folder to cater for possible conflicts.
		1.3. Import the new version.
		1.4. Try out the Demo scene for getting started information.

For full setup instructions visit: http://www.flipwebapps.com/game-framework/setup/		
		
- - - - - - - - - -

CREDIT

ConditionalHideAttribute & PropertyDrawer - by Brecht Lecluyse (www.brechtos.com)
		
- - - - - - - - - -

CHANGE LOG

v1.3
	Game Framework Core - Improvements
	- BeautifulTransitions: Updated to v1.2 that includes:
		- New Rotate UI / Game Object transition. 
		- Transition core code refactor.
		- Added the ability to define your own animation curves.
		- Added Custom Property Editor for improved UI and additional help. 
		- Various fixes (see seperate 'Readme - Beautiful Transitions.txt' for full details)
	- Display: Added SetQuadUVs
	- Editor: Added Conditional Hide property drawer (thanks to Brecht Lecluyse -  www.brechtos.com). 
	- Editor: Moved Editor files to allow better grouping
	- GameObjects: Added MinMax and MinMaxf structures.
	- GameStructure: Added Weighting framework for managing relative object weights over distance / time 
	  and selection of items based upon this. Usage will be demoed in a full infinate scroller game template in the 
	  paid version.
	- GameStructure: Allow specifying of an optional game item specific scene from game item button (e.g. for level).
	- Input: New shared OnMouseClickOrTap base class.
	- UI: Improvements for dialog swapping / transitioning
	- UI: Added OnButtonClickSwapDialogInstance and OnMouseClickOrTapSwapDialogInstance components.

	Game Framework Core - Fixes
	- UI: DialogInstance IsShown state correctly reflects active value

	Game Framework Extras (Paid Version Only) - Improvements
	- Scrolling: Added Scrolling script including manual / automatic scene setup, pooling and reuse of display instances, 
	auto scroll of follow camera, parallex support, for automatic scene setup - relative weightings for displayed gameobjects given 
	distance or time.

	Game Framework Tutorials (Paid Version Only) - Improvements
	- The start of what will be a full 2D infinate scrolling game template with tutorials. (work in progress)

v1.2
	Game Framework Core - Improvements
	- Input: New shared OnMouseClickOrTap base class.
	- UI: Improvements for dialog swapping / transitioning
	- UI: Added OnButtonClickSwapDialogInstance and OnMouseClickOrTapSwapDialogInstance components.

	Game Framework Core - Fixes
	- UI: DialogInstance IsShown state correctly reflects active value

v1.1
	Game Framework Core - Improvements
	- Removed FadeLevelManager as more advanced screen wipes are supported through the bundled BeautifulTransitions asset (paid verions only)
	- Updated the bundled BeautifulTransitions asset to v1.0 that includes:
		- Rewritten core for easier extensibility
		- Added Screen transitions including fade and multiple wipe transitions
		- Added Camera transitions including fade and multiple wipe transitions
		- Added the possibility to create your own custom transitions by uploading a new Alpha texture
		- Added new demo for screen and camera transitions.
	- Updated Demo scene

	Game Framework Extras - Fixes
	- Dialog fixes for updated transitions

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