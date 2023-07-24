# ScriptableObject Oriented Game Architecture

This project is a set of Unity scripts and tools designed to facilitate the use of ScriptableObjects as a core element of game architecture. Inspired by Ryan Hipple's GDC talk in 2017, it provides a practical implementation of the principles he described.

## Features

- **Shared Variables:** Create ScriptableObject instances to hold shared variables of various types. These variables can be referenced and manipulated by multiple game objects and scripts, allowing for decoupled and flexible design.

- **Shared Variable Getters:** Components that can be attached to game objects to automatically fetch and update shared variable values, simplifying the process of keeping game state consistent across different parts of your game.

- **Variable References:** These allow you to use either a constant value or a reference to a shared variable, providing greater flexibility in how you use variables in your game.

- **Event Systems:** Leverage the power of Unity Events, but with a more flexible and decoupled design, thanks to the use of ScriptableObjects.

- **Custom Editor Tools:** Generate necessary boilerplate scripts to support new shared variable types or extend the SEvent class with a custom editor window.

## Usage

To use this project, import the provided scripts into your Unity project. Once the scripts are imported, you can create Shared Variable ScriptableObjects from the Unity editor, attach getters to your game objects, and more.

For more detailed instructions on using each feature, see the included example scenes and scripts.

## Contributing

Contributions to this project are welcome! Please submit a pull request with any improvements or fixes you have made.

## License

This project is licensed under the MIT License. Please see the LICENSE file for more details.

