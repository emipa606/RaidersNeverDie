# GitHub Copilot Instructions for Raiders Never Die (Continued) Mod

## Mod Overview and Purpose
**Raiders Never Die (Continued)** is a mod for RimWorld that alters the default death mechanics for raiders, ensuring they are less likely to die compared to the vanilla experience. By default, raiders in RimWorld have a higher chance of dying than colonists even when using non-lethal methods. This mod allows users to modify the additional death risk for raiders, providing a more balanced experience. Updating the original concept by schplorg and sems, this mod is maintained for compatibility with the latest game versions and is configurable via mod settings to tailor gameplay to individual preferences.

## Key Features and Systems
- **Customizable Death Risk for Raiders**: The mod removes the inherent additional death chance for raiders, with in-game settings allowing users to adjust this feature.
- **Separate Configurations for Animals and Mechanoids**: With the latest updates, separate sliders have been introduced for tweaking death chances of animals and mechanoids.
- **Compatibility and Bug Fixes**: Regular updates are made to ensure compatibility and address issues, such as the way this mod affects animals and mechanoids and previous bugs with Harmony method usage.

## Coding Patterns and Conventions
- **Project Structure**: 
  - C# scripts are organized by functionality. Key files include `HarmonyPatches.cs`, `RNDSettings.cs`, `RNDSettingsController.cs`, and `Pawn_HealthTracker_CheckForStateChange.cs`.
- **Naming Conventions**: 
  - Classes and methods use PascalCase while local variables follow camelCase. This aids in distinguishing between different types of identifiers.
- **Commenting Style**: 
  - Use XML documentation comments for classes and public methods to ensure automatic documentation generation and improved code readability.

## XML Integration
While the current version does not include XML files, future extensions might require XML-based Defs for more extensive game modifications. In such cases:
- Define Defs in XML to override or extend game definitions.
- Ensure XML format adheres to RimWorld's specific schema for seamless integration.

## Harmony Patching
- **Purpose**: Harmony is used for non-intrusive alterations to the game code. This ensures compatibility and allows the mod to only target specific game behaviors.
- **Key Patch**:
  - `Pawn_HealthTracker_CheckForStateChange.cs` includes a Prefix method for altering the logic before the game's original method execution, adjusting death probabilities.

## Suggestions for Copilot
- **Pattern Recognition**: Provide suggestions for added Harmony patches by recognizing existing patterns within `HarmonyPatches.cs`. Enhance these with similar method structures.
- **Refactor Code**: Use Copilot to identify repetitive code blocks and suggest refactoring opportunities for modular and maintainable code structures.
- **Comments and Documentation**: Generate useful inline comments and ensure public methods and classes are documented with XML comments.
- **XML Extensions**: Suggest XML templates and structures for potential XML integrations based on RimWorld's modding documentation, should future updates require it.

*For best practices in modding and more tailored suggestions, please ensure the proper understanding of RimWorld's source code structure and Harmony documentation.*

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

