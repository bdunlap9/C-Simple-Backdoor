# C-Simple-Backdoor
C# Backdoor 

Enables Remote Desktop Protocol via Registry
Enables Remote Assistance via Registry

Uses an API to get external IP then writes it to out.txt which will later be emailed to the desired gmail account.

After you recieve the email take the IP written in the attahced file and open up the command prompt as Administrator then type telnet and proceed to connect to the IP or hit the Windows key + S and type in RDP and use the RDP tool to connect to that IP.

Notes:
12/10/2017 - Fixed email function, after getting external ip and outputting to out.txt it will now email it to your email address (did not work properly before but has now been fixed.)
