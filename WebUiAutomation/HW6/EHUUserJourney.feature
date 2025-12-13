Feature: EHU University Website User Journey
  As a prospective student
  I want to browse the EHU University website
  So I can change language, search programs, read about the university and find contact details

Background:
  Given I open the EHU main page
  And I accept cookies if they appear

Scenario: User changes website language to Lithuanian
  When I open the language selector
  And I select Lithuanian language
  Then the Lithuanian version of the website should be opened

Scenario: User searches for study programs
  When I open the search panel
  And I search for "study programs"
  Then search results page should display study programs

Scenario: User navigates to About page using footer
  When I navigate to About page from the footer
  Then About page should be opened
  And the about page title should contain "About"

Scenario Outline: User verifies contact information
  When I open the Contact page
  Then contact element "<selector>" should contain "<text>"

Examples:
  | selector                               | text                                         |
  | a[plerdy-tracking-id='35448735101']    | franciskscarynacr@gmail.com                  |
  | li[plerdy-tracking-id='50296369501']   | +370 68 771365                               |
  | li[plerdy-tracking-id='39744896801']   | +375 29 5781488                              |
  | li[plerdy-tracking-id='64965466401']   | Join us in the social networks               |
