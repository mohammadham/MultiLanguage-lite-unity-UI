# MultiLanguage-lite-unity-ui
Support multi-language in UI for 2D/3D games in Unity.

## Scripts Overview

### LanguageUi.cs
- **Description:** Responsible for managing UI text elements for multi-language support.
- **Components:** Handles Text and TextMeshPro UI elements based on the active language.
- **Functionality:** Interacts with LanguageManager to get translated text for the specified language.

### LanguageTextMeshProUi.cs
- **Description:** Manages TextMeshPro UI elements for multi-language support.
- **Functionality:** Provides a method to change text content based on the language.

### LanguageTextUi.cs
- **Description:** Manages Text UI elements for multi-language support.
- **Functionality:** Provides a method to change text content based on the language.

### LanguageManager.cs
- **Description:** Handles loading and management of language data.
- **Components:** Contains methods to load language configuration files and language data files.
- **Functionality:** Provides language data based on the active language and loads language files from specified paths.

### ConfigLanguages.cs
- **Description:** Defines the structure for language configuration data.
- **Components:** Contains the active language name and language file paths.

### Language.cs
- **Description:** Represents a language object with its name and associated words.
- **Components:** Contains a dictionary of words in the specified language.

## File Structure
- LanguageUi.cs: Manages UI text elements for multi-language support.
- LanguageTextMeshProUi.cs: Handles TextMeshPro UI elements for multi-language support.
- LanguageTextUi.cs: Manages Text UI elements for multi-language support.
- LanguageManager.cs: Manages language data loading and retrieval.
- ConfigLanguages.cs: Defines the structure for language configuration data.
- Language.cs: Represents a language object with associated words.

## Usage
- LanguageManager.Instance: Singleton instance for managing language data.
- LanguageManager.LoadConfigFileData(): Loads language configuration data from a JSON file.
- LanguageManager.LoadFileData(string fileName): Loads language data from a specified file.
- LanguageUi: Manages UI text elements and interacts with LanguageManager for language translations.

## Note
Ensure to set up the language configuration files and language data files as per the specified structure for proper functionality.

## Multi-Language Support Scripts Overview
### How They Work
- LanguageUi.cs: Manages UI text elements and interacts with LanguageManager to retrieve translated text based on the active language. It dynamically updates text content for different languages.
- LanguageTextMeshProUi.cs: Handles TextMeshPro UI elements by changing the text content based on the selected language. It ensures seamless integration of multi-language support with TextMeshPro.
- LanguageTextUi.cs: Manages Text UI elements and updates text content according to the active language. It provides a simple way to handle multi-language text in UI elements.
- LanguageManager.cs: Centralizes the loading and management of language data. It loads language configuration files and language data files, retrieves language data based on the active language, and ensures proper handling of language-related operations.
- ConfigLanguages.cs: Defines the structure for language configuration data, including the active language name and language file paths.
- Language.cs: Represents a language object with associated words, allowing for easy access to translated text in different languages.

### Why They Are Useful
- Centralized Language Management: Provides a centralized approach to managing multi-language support in the project, making it easier to handle translations and language-specific content.
- Dynamic Text Updates: Users can switch between languages, and the scripts dynamically update the text content in UI elements without the need for manual intervention.
- Flexible Integration: Easily integrated into existing projects, allowing developers to add multi-language support without significant modifications.
- Efficient Translation Handling: Efficiently handle language-specific content and ensure a seamless user experience by using the LanguageManager to retrieve translated text.
- Customizable Configuration: Allow customization of language configuration files and provide flexibility in managing language data, making it adaptable to different project requirements.

These scripts offer a robust solution for implementing multi-language support in Unity projects, providing efficient text localization and seamless language switching capabilities for enhanced user interaction and accessibility.
