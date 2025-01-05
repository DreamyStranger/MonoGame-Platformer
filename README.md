# MonoGame-Platformer

Implementation of a mix of Entity-Component framework approach with object-oriented programming (OOP) using MonoGame.

Assets Used: [Pixel Adventure 1](https://pixelfrog-assets.itch.io/pixel-adventure-1)

Documentation (Doxyfile): [ECS-MonoGame Documentation](https://dreamystranger.github.io/ECS-MonoGame/annotated.html)

## Running the Code

Follow the instructions below to run the code successfully:

### 1. **Download the Project**
   - Download the project as a ZIP file or clone it using GitHub Desktop.

### 2. **Open the Project in VS Code**
   - Open the project in Visual Studio Code (VS Code).

### 3. **Install C#**
   - Ensure that C# is installed in your VS Code environment.
   - Install C# using the following command:
     ```bash
     code --install-extension ms-dotnettools.csharp
     ```
### 4. **Install the .NET 6 SDK**
   - Make sure the .NET 6 SDK is installed

### 5. **Install MonoGame Templates**
   - Install the MonoGame Templates by using the following command:
     ```bash
     dotnet new --install MonoGame.Templates.CSharp
     ```
     
### 6. **Install SpriteFontPlus**
   - For font loading, you might need to install `SpriteFontPlus`. Use the appropriate dotnet command to install it.

### 7. **Run the Code**
   - Ensure all prerequisites are met to run the code successfully.


## Keyboard Controls

The keyboard controls for the product are as follows:

- Use the **A** and **D** keys to move left and right, respectively.
- Press the **SPACE** bar to perform a jump.
- To navigate between levels, use the following keys:
  - Press **N** to go to the next level.
  - Press **P** to go to the previous level.
  - Press **R** to reload the current level.

## Navigating the Project

When exploring the project, you will come across the following folders:

1. **ContentManager**

   The "ContentManager" directory serves as the project's content management system. It is responsible for managing and loading various game assets such as graphics, audio files, and other resources. Additionally, the ContentManager also reads and creates entire levels from a Tiled map format, providing a convenient way to design and populate game levels.

2. **ESC base**

   The "ESC base" directory contains the core components and systems of the Entity-Component System (ECS) framework. Here, you will find the essential building blocks for creating and managing entities, components, and systems.

3. **ComponentManager**

   The "ComponentManager" directory refers to the folder that manages the components within the ECS framework. If any changes or updates are needed for the components, this is where you should make those modifications.

4. **SystemManager**

   The "SystemManager" directory houses the system management functionality of the ECS framework. Any minor changes or adjustments to the systems should be implemented within this directory.

5. **EventManager**

   The "EventManager" utilizes the MessageBus class for publishing messages. This directory handles event handling and manages the communication between various game components and systems through the MessageBus. Any modifications or additions related to event messaging should be made within this directory.

6. **ObjectManager**

   The "ObjectManager" directory contains the Entity Factory, which serves as a central point for creating and managing entities in the game. If you need to add a new type of entity, this is where you should start. Modify the Entity Factory to include the creation logic for the

7. **WorldManager**

    The "WorldManager" directory pertains to the management of the game world within the project. It handles aspects such as scene management, level loading, and other world-related functionalities.

## Status: Stopped

Since the school switched to Godot and Unity, I am not gonna develop this template further. I may instead just provide Unity and Godot examples in the future.
