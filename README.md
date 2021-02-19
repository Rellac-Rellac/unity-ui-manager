# unity-ui-manager
A Simple UI Manager for rapid prototyping and ease of collaboration

![](gif_demo.gif)

# Features
-----------
- Extremely modular - forget about merge conflicts!
- Very fast to get a prototype up and running
- Pretty UI Transitions just as easy as GameObject.SetActive()
- Very simple inspector with minimal inputs, a great way to bridge the experience gap between a developer and a designer
- Only 5 small scripts control the whole system - not much can possibly break
- Use of Animations give an incredible amount of control over transitions
- No code required

# UI Manager
------------
**Create > UI > Manager**

The UI Manager is used to instantiate a starting point and flip between panels in that RectTransform. Multiple managers can be used throughout the project and they can reference any UI Panel object.

A Manager must first be Initialised with a starting RectTransform before panels can be called.

![](Init-example.png)

An initialised Manager can swap between panels via SetPanel(UIPanel)

![](SetPanel-example.png)

# UI Panel
----------
**Create > UI > Panel**

UI Panels are used to reference the panel prefab and the transition animation used for each panel.

Assign a prefab here to instantiate whenever this UIPanel is called - root will be adjusted to a stretched fit over the target RectTransform of the UI Manager

Specify here the transition animation that you would like to use. The animation must be listed in the Animator Controller of the UIRoot prefab

The speed of the transition is specified in seconds and you can enable the passover click blocker for your smaller panels to prevent clicking unwanted background buttons

Events are available for whenever this panel is instantiated and when the transitions in/out have ended

![](UIPanel-example.png)

# UI Panel Listener
--------------------
UIPanelListener can be added to any GameObject and will listen for the relevant events of any specified panel. This can be used to trigger an animation within the panel once it has transitioned or to hide a GameObject until the transition has finished.

Available events to register to:
![](Listener-example.png)

# Adding an Animation
----------------------
Animations can be added to the prefab located at UIManager/Resources/UIManager/UI Root

Do not adjust the sizeDelta in your animation - this value is adjusted by the UIFitter on the UI Root prefab. It will automatically adjust the Rect to fit the specified parent Rect

The "Parent_In" object is the panel that is currently coming in and will end as the main panel the user is looking at

The "Parent_Out" object is the panel that is currently already in and will end out of sight of the user

If a new GameObject is required to create your bespoke transition, make sure that the starting pose is in its "off"
state so that other animations don't need to reference it - see the behaviour of the currently disabled GameObjects
under the UI Root prefab animations

Animations should last 1 second each so that we can specify the speed in seconds - don't worry if the transition is
too fast at 1 second - simply specify a larger time in your UIPanel Object
