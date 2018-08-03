# Keep Backend - Similar to Google Keep

**Objective:** Create an Application which should be able to drive the Google Keep Application.

The intent of the application is to use **ASP .NET Core Web API's** and **Entity Framework Core** 

## Google Keep Specs
- A Note can have potential things:
  - Must have a **title**
  - Can have **plain text**
  - Can have **checklist**, basically a list of items
  - Can have a set of **labels**
  - Can be **pinned**
- Should be able to **create** notes
- Should be able to **retrieve** all/individual notes
- Should be able to **delete** a set of/individual notes
- Should be able to **edit** an individual note
- Should be able to **retrieve** notes by labels
- Should be able to **retrieve** **pinned** notes
- Should be able to **search** the notes by **title**

## Instructions

- Create a Web API Application which satisfies the above specs
- Create a Documentation using Swagger for the above specs
- Write Test Cases which should test for the above specs
- Configure Continuous Integration with this application