# Reactive Fields Updater

This XrmToolBox tool provides a smart interface to update the Field Attributes of your Power Apps Entities.

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Installation](#installation)
- [Framework dependencies](#framework-dependencies)
- [Release notes](#release-notes)
- [License](#license)

## Description

Reactive Fields Updater is thought to update the Fields Attributes for specific Entities in Power Apps.
You can easily increase or decrease Attributes such as Max Length on a Text field, or Max and Min Value on numeric fields.

Start by loading your Entities from the Organization you're connected to (they are loaded automatically when a connection is available).
Then select an Entity and a Field from that Entity. In the right corner, the Attributes box displays the current values of the Attributes you can modify: type the new value in the "New Value" column.
Every change is listed as a new Operation in the box below.
You can check and uncheck the operations that you want to execute, and then just click "Update!". You can also remove the checked operations with the "Clear" button.

Imagine you have a set of 20 custom Entities and you need to increase every "Name" Max Length from 100 to 800:
Reactive Fields Updater is a valid alternative to Power Apps when it comes to multiple or repetitive operations to be performed at once, since multi-entity operations are supported.

## Features

- Supported field types: Text, Multiline Text, Whole Number, Decimal, Floating Point and Currency.
- Editable properties: `MaxLength`, `MinValue`, `MaxValue` (configurable in the settings file).
- Filter boxes and sortable columns (click the header) for Entities and Fields.
- The Attributes grid shows the current value and the new value; invalid values (wrong type, MaxLength out of range) are rejected while typing.
- New operations are selected by default; press `Del` to remove the highlighted operations, `Ctrl+A` to select all of them.
- Operations on the same field are sent in a single request; failed operations stay in the list with their error message (hover to read it), successful ones are removed.
- Optional "Publish after update" toggle in the toolbar.
- Changing connection resets the tool; closing it with pending operations asks for confirmation.

## Installation

This tool can be downloaded from the XrmToolBox Tool Library. Connect to your Organization, then you're ready to go.

The user needs the privileges to customize the system (e.g. *System Customizer* or *System Administrator* security role).

## Framework dependencies

| Dependency | Version |
| --- | --- |
| .NET Framework | 4.6.2 |
| XrmToolBox (`XrmToolBoxPackage`) | 1.2022.10.58 or later |
| Dataverse SDK (`Microsoft.CrmSdk.CoreAssemblies`) | 9.0.2.46 |
| `MscrmTools.Xrm.Connection` | 1.2022.10.54 |
| Dataverse / Dynamics 365 | v9.x (online or on-premises) |

To build the project you need Visual Studio 2019 or later with the .NET Framework 4.6.2 targeting pack; NuGet packages are restored from `packages.config`.

## Release notes

### 1.2.0
- **New:** filter boxes and sortable columns for Entities and Fields.
- **New:** Attributes grid with "Current Value" / "New Value" columns and input validation.
- **New:** support for Multiline Text, Decimal, Floating Point and Currency fields.
- **New:** "Publish after update" option, confirmation before updating and summary afterwards.
- **New:** keyboard shortcuts in the Operations list (`Del`, `Ctrl+A`); new operations are checked by default.
- **New:** entities are loaded automatically; changing connection resets the tool; confirmation when closing with pending operations.
- **Improved:** field metadata is cached and operations on the same field are sent in a single request, with progress messages.
- **Fixed:** error when clearing the Attributes grid after loading entities, fields or clearing operations.
- **Fixed:** check state of the operations lost when a new operation was added.
- **Fixed:** failed operations were removed from the list; now they stay with their error message.
- **Fixed:** operations and settings were shared among multiple instances of the tool; settings were not saved on close.

### 1.1.4
- Responsive layout.

### 1.1.2 - 1.1.3
- "Select All" / "Unselect All" links in the Operations box and UI improvements.

### 1.1.1
- Buttons order.

### 1.1.0
- First public release: operations list with checkboxes, toolbar and settings.

## License

This project is licensed under the [MIT License](LICENSE).
