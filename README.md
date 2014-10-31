Whitelog
========

Whitelog is a next generation logging utility.

logging was never a sexy feature in the programing world and we all use tools like Log4Net or Microsoft Enterprise Library that gave us some flexibility and abstruction over the loggin task, but it always felt like we are using a 20 century tool in the 21 century world.


Whitelog was design to change all of that:
* No more configurations over xml -> We like code, it always clearer and easier.
* No more override ToString() to format your logs -> Fluent definition of log data.
* No more compromise log writes for readability/performance -> Write every thing, cost is negligible.
* No more finding a needle in a haystack -> Hierarchical logging will help you find what you are looking for in a tree structure.
* No more notepad -> Powerful viewer is constructed to view logs in extremely comfortable way(Barak Log Studio*).
* No more bloated log files -> Binary appender will use advance serializing to significantly reduce file size while still remain highly compressible using simple compression algorithm(zip).
* Optimistic lock free implementation and zero memory allocation during log writing**.
 

Status
======
The current status of the project is an early alpha, Interfaces are not guaranteed not to change.
<a href="http://ronenbarak.no-ip.org:8086/viewType.html?buildTypeId=btN&guest=1"><img src="http://ronenbarak.no-ip.org:8086/app/rest/builds/buildType:WhiteLog_BuildAllAndTests/statusIcon"/></a>

Licence
=======
We use the Apache License, Version 2.0


Remarks
-------
*Barak Log Studio- This project is under heavy development and currenlty not open source but it will be completely free.

** Lock free and zero memory allocation is only true for specific high performance configurations, most configurations are slightly less efficent but have less foot print on the entire application when idle and more recommended.

