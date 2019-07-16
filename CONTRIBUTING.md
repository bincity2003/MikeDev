# Contribution Guidelines
Welcome to the Contribution Guidelines for MikeDev, a collection of useful libraries that help life easier.

## Introduction
First off, thank you for considering contributing to our repository. It's people like you that make it such a great tool.

Following these guidelines helps other developers manage this open source project better.
In return, we will help you in addressing your issues and finalizing your pull requests.

MikeDev is an open source project and we love to receive contributions from our community — you!
There are many ways to contribute, from writing and improving the documentation,
submitting bug reports and feature requests or writing code which can be incorporated into MikeDev itself.

**We are looking for your help**. Everything is appreciable!

## Your responsibilities
Once you've decided to contribute, please remember to follow these rules:
* Ensure cross-platform compability for **every single change** that's accepted (Windows, Mac, Ubuntu Linux).
* Ensure that your code follows our [Coding Style]().
* Before changing anything, check the
[Issue Tracker](https://github.com/bincity2003/MikeDev/issues)
first whether someone is working on it.
If not, you **must** open an issue (use existing templates)
* **Never, ever** add new classes (except for custom exceptions) to the codebase.
* Think before adding new dependencies, especially to existing libraries.
* Be welcoming to newcomers and encourage diverse new contributors from all backgrounds.
See the [Python Community Code of Conduct](https://www.python.org/psf/codeofconduct/).

## Your first contribution
Is this your first contribution? Then search for
["Newcomers" issues](https://github.com/bincity2003/MikeDev/issues?q=is%3Aissue+label%3Anewcomers).
If there is any, work on it! If there isn't, make a new issue (use existing templates), then work on it or ask others to help you!

At this point, you're ready to make your changes! Feel free to ask for help; everyone is a beginner at first.

If a maintainer asks you to "rebase" your PR, they're saying that a lot of code has changed.
Then you need to update your branch so it's easier to merge.

Having troubles? Feel free to ask us or refer to
[*this guide*](https://egghead.io/courses/how-to-contribute-to-an-open-source-project-on-github).

## Getting started
#### For something that's smaller than a one or two line fix:
It's usually for spelling, grammar in code's comments or documentation
1. File [a new regular issue](https://github.com/bincity2003/MikeDev/issues/new/choose)
and use the tag "Small change", plus other necessary tags.
2. State the followings:
    * What's your issue?
    * Where do you see that?
    * (*Optional*) Do you know how to fix it? If yes, how?
    
If you answer "Yes", please do the following:
* Create a new branch, called ```<ClassName>-<issue>-fix```. For example, ```dbtable-grammar-fix```.
* Commit your changes to this branch!
* After making sure that it's fixed, make a pull request to ```master``` branch, declaring the same things in your issue, plus:
    * How did you fix it?
    * Confirm that you didn't change anything else!

If you answer "No", use the tag "Need help" and wait for someone to do it for you.
After 7 days, if no one haven't response yet, mention anyone that can help you.

#### For something that is bigger than a one or two line fix:
Whatever it is, this is the general rule:
1. Create your own fork of the code.
2. Do the changes in your fork.
3. If you like the change and think the project could use it:
    * File a new issue (more details later)
    * Send a pull request to our repository (also, more details later)

## "Big" changes
Everything else, adding/improving documentation, reporting bugs, adding/improving features, should come in this category.
### How to report a bug
***CAUTION***: If you found a security vulnerability, **DO NOT** open an issue. Please email me at bincity2003@outlook.com!  

When filing a bug report, please follow the
[Bug Report](https://github.com/bincity2003/MikeDev/issues/new?template=bug_report.md) template.
### How to suggest/implement a feature or an improvement
*MikeDev* philosophy is to provide necessary, robust but easy-to-use tools for use in any other projects,
making it a great part in any application's backend library.
Therefore, please think for not only your benefit, but others too, before suggesting anything.
#### Suggestion for us
If you can't do it, let us know!  
As before, please follow the
[New feature](https://github.com/bincity2003/MikeDev/issues/new?template=feature_request.md) or
[Improvement](https://github.com/bincity2003/MikeDev/issues/new?template=improve_request.md) templates.
#### Self-implementation
If you can do it yourself, please go ahead! But remember, you can always ask for help by mentioning others.
Once again, follow the above templates.
## Coding style
This is one of the most important parts, Coding Style!  
Rules:
* First off, follow the C# naming convention
(except for internal data holders or methods, you can prefix it with ```_``` or ```_Internal```).
* Again, minimise dependencies and classes. Don't make anything unnecessary!
* Use 4 *space* only (or set your "tab" to 4 spaces) for indentation.
* Each line of code should not be longer than 120 characters.
* (For VS2019 user) Before committing your work, you can run the following Code Cleanup profile (not anything else):
    * Sort usings
    * Remove unnecessary usings
    * Remove unnecessary casts
    * Add accessibility modifiers
    * Sort accessibility modifiers
    * Add/remove braces for single-line control statements
* (For non-VS2019 user) You have to all of the above (not anything else), manually!
* If possible, only two classes per feature (1 for the feature itself, 1 for custom exception during feature's execution).
If you use more than 2, please **state** it clearly in your pull request.
## Thank you
Once again, thank you for your contribution! As a rule of thumb, always follow these guidelines when working.
