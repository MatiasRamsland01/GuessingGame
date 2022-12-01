# Workflow: Github branches and pull requests
* Before starting to code on an issue
  - Start by making a new branch. <strong>Never work on main branch</strong>
    - ```git checkout -b yourbranchname```
  - This automatically creates a new branch and selects it. Here you can code and work on your issue. You can pull and push from this branch just like any other branch
  - To revert back to a given branch: 
    - ```git checkout branchname```
  - When you are finished you can push your code to your created branch
    - ```git commit -am "your commit message```
    - ```git push --set-upstream origin yourbranchname```
- Start by making a pull request
  - Press on pull request 
  - Press on "New Pull Request"
  - Base = the branch that is being merged to
  - Compare = the branch that is being merged from
  - Usally Base=main and compare=yourbranchname
  - Press create pull request
  - On the right hand side you can select reviewers if you want someone specific to llok at your code.
  - Press create pull request
  - Now the pull request is created and now someone else have to look at your code and accept the pull request.
  - <strong>Do not accept your own!</strong>
- Look through and accept a pull request
  - In the menu to the pull ruquest on can look through the code by pressing files changed
  - Hovering over the lines of code you can insert a comment if needed. 
  - If this pull request looks good you press review changes
    - Leave a comment
    - Now you have the option to "approve" so merge the branches
    - Or "request change" if there are any that needs fixing
  - If you wish to see the changes locally you can use ```git checkout --track origin/NameOfBranch``` to change to the remote branch

## Please ask if there are any questions

* Hints
  * One can connect an issue to a branch/pull request so it automatically gets moved to done in github projects
