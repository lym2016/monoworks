# Introduction #

The following outlines the various components of the Framework UI system and how they can be used to practically create a Framework application. Note that all code shown (and more) is a part of the demo application included with the Framework source.

# The UI File #

The UI file is an XML file that describes the basic user interface of a Framework application window. This includes:
  * Actions (user events that have an associated name, icon, and tooltip)
  * Menus
  * Toolbars
  * Toolboxes (custom tool containers similar to toobars)
  * Document Types
  * Dockable Layout

The following sections will describe each of these components in more detail.

# Actions and the Controller #

Actions are the foundation of Framework applications. They define a specific user interface event that is handled by the controller. The controller is an object that receives all actions for a particular window and handles them appropriately.

An action has the following attributes:
  * name - the name used to refer back to the action by other UI elements
  * icon - the name of the icon used to represent the action (optional)
  * tooltip - the tooltip text displayed when the user hovers over the action's control (optional)

All action declarations must go inside an Actions tag in the UI file. A common action declaration may look like this:
```
<Actions>
    <Action name="New File" icon="file-new" tooltip="Create a new file"/>
    <Action name="Open File" icon="file-open" tooltip="Open an existing file"/>
    ... more actions ...
</Actions>
```


# Menus #

Menus are defined inside a Menus tag. Each menu has a name and a list of items (corresponding to actions).
```
<Menus>
    <Menu name="File">
        <Item action="New File"/>
        <Item action="Open File"/>
        ... more items ...
    </Menu>
    ... more menus ...
</Menus>
```


# Toolbars #

Toolbars are defined inside a Tools tag. Each toolbar has a name and a list of items (corresponding to actions). It must also have a position tag, defining its position around the edge of the window (Top, Left, Right, or Bottom).
```
<Tools>
    <Toolbar name="File" position="Top">
        <Item action="New File"/>
        <Item action="Open File"/>
        ... more items ...
    </Toolbar>
    ... more tools ...
</Tools>
```