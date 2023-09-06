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

Unit tests have been added, however for some reason... they do not see mocked database, and, as result, most of them refuses to work properly. In other case however all 6 minut tests would've most likely been passed.
How to use it?

To run application you will have to:
- change a database server to your own (preferably MSSQL Server) in appsettings.json:
"BookingSystemContext": "Server=DESKTOP-LFET3LQ\\LINGUONATOR;
- then use command update-database
- create new account, to make it easier each new account is given highest permission level (that can be later edited with put command)
- log in with newly created account
-  proceed authentification with JWT token given in response body
-  click "Authorize" in upper right corner, paste token in and accept
Now you can use the application.

User:
Allows for creating account and logging in with JWT security token which allows users to access specified endpoints of the app. It also delete outdated reservations upon logging in.
Location:
Nothing out of ordinary. Cannot be deleted if there's at least one desk assigned to it.
Desk:
Same with Location, however admins can make desks unavailable at will.
Reservations:
Reservations can only be edited if edit takes place at least 24h before beginning of reservation. Otherwise such reservation can only be deleted. Edit option allows user to change being date, end date and/or desk. Get endpoint is accesible by any logged in user, however at least admin is required to see full information about desk.

Well, that's all. Sadly lots of things are missing due to either my lack knowledge (forntend) or technical issues (unit tests, services). I hope you will find that small project decent nonetheless.
