Eine WCF Awendung in der verteiltes Rechnen mit nodes dafür verwendet wird Aufgaben die per Plugin erweitert werden können
zu berechnen.
Mit Beispielplugins Pierpontprimes berechnen und Primfaktorzerlegung
Plugin Erweiterbarkeit in Runtime ohne Neustart mit .Net MEF Framework

Der Aboslute Pfad in WcfServiceLibrary1 -> ServiceManager pluginDir in Zeile 30 muss geändert werden
sodass er auf VerteilteSysLib/VerteilteSysHost/ExternalPlugins zeigt.

Die Plugin dlls sind im Plugins Ordner vorhanden.
Um die Anzahl der verwendeten Nodes der Plugins zu ändern in den Pluginklassen in nodeAmount nodeAmount verändern und neu builden
Die Plugins Builden in den Standardpfad, hier kann die dll herausgenommen werden um zu uploaden
Vor dem erneuten Upload müssen die alten Versionen der Plugins aus dem Ordner
"C:\Users\micon\Desktop\Michael Sievers 690593\VerteilteSysLib\VerteilteSysHost\ExternalPlugins"
und
"C:\Users\micon\Desktop\Michael Sievers 690593\VerteilteSysLib\Node\bin\Debug\Extensions"
gelöscht werden.
Die Plugins inkl Ordner müssen gelöscht werden also zb
"C:\Users\micon\Desktop\Michael Sievers 690593\VerteilteSysLib\Node\bin\Debug\Extensions\c5f79251-97a0-49c9-ae6b-4157ec2d73e4"
Löschen.

Startreihenfolge:
Host -> Als Admin starten
Node
Client

Es können jeder Zeit mehr Knoten gestartet werden
Zum Starten von mehr Knoten nach dem Starten des Servers 
"C:\Users\micon\Desktop\Michael Sievers 690593\VerteilteSysLib\Node\bin\Debug\Node.exe"
ausführen

Upload im Client:
upload [Enter]
dann Pfad angeben