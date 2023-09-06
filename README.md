# BookingSystem
a task for software mind internship
since it's 6am, I will jus twrite a quick readme, more detailed one will appear later.

This application offers everything that has been required to be implemented:
## Requirements
Administration:
- Manage locations (add/remove, can't remove if desk exists in location)
- Manage desk in locations (add/remove if no reservation/make unavailable)
Employees
- Determine which desks are available to book or unavailable.
- Filter desks based on location
- Book a desk for the day.
- Allow reserving a desk for multiple days but now more than a week.
- Allow to change desk, but not later than the 24h before reservation.
- Administrators can see who reserves a desk in location, where Employees can see only that specific desk is unavailable.

Also also few more features:
- auto-deletion of outdated reservations and reseting desks as available
- a possiblity to make a new account
- owner account which allows to modify existing members (this is not controlled at all and advised to use it only when user is aware of their actions)
(admin and owner for now have to be added manually via MSSQL, but I will change this today)

For now application only lacks unit tests, but they will be added today.

In order to test all the user needs is a local MSSQL server, add its path in appsettings.json, type in a packet manager console update-database and boom, it's ready to be tested.
