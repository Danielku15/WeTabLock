WeTabLock
=============
[![Screenshot](http://img193.imageshack.us/img193/1954/lockscreenw.th.jpg "Screenshot")](http://img193.imageshack.us/i/lockscreenw.jpg/)

Ziel dieser Anwendung ist es eine Tastensperre für das WeTab zu erstellen. 

Sperren
-------
Wird der Powerbutton auf der Rückseite des WeTab’s betätigt, wird der Bildschirmschoner aktiviert. Als erste Routine wird der Bildschirm komplett Ausgeschaltet um Strom zu sparen und eine Art „Tastensperre“ zu bewirken.   

Lockscreen
----------
Die Reaktivierung geschieht nachdem der Bildschirm auf irgendeine Weise unterbrochen wird wie etwa:

* Betätigen des Powerbuttons
* Betätigen des Sensorbuttons
* Bewegen einer externen Maus
* Betätigen einer externen Tastatur

Nach der Reaktivierung wird der Lockscreen angezeigt. Wird nach einer gewissen Zeit nichts auf dem Lockscreen betätigt, so wird die Bildschirmsperre wieder aktiviert.  Der Lockscreen besteht aus 3 Bereichen: 

* Infoleiste mit Systeminformationen (Zeit, Datum, Akku, Wifi, 3G,…)
* Der Widget Bereich
* Die Entsperrleiste

Wird über die Entsperrleiste die entsprechende Aktion durchgeführt, so wird der Lockscreen beendet. 

Sperrmodi
---------
### Apple Modus
Der alt Bekannte „Slide to Unlock“ Modus: Wird der Balken nach links geschoben, wird entsperrt.

![Apple Modus](http://img696.imageshack.us/img696/4854/unlockslide.jpg "Apple Modus")

### Zahlencode Modus 
Wird ein 4 stelliger Code richtig eingegeben, wird entsperrt. 

![Zahlencode Modus](http://img575.imageshack.us/img575/8287/unlocknumbers.jpg "Zahlencode Modus")

### Pull Modus 
Wird die Lasche aus der Seite gezogen, wird ensperrt.

![Pull Modus](http://img186.imageshack.us/img186/2632/unlockpull.jpg "Pull Modus")
 
### Einstellungsdialog 
Im Einstellungsdialog können folgende Einstellungen getätigt werden: 

* Hintergrundbild festlegen
* Angezeigte Infos in der Infosleiste
* Angezeigte Widgets und deren Anordnung
* Verwendeter Entsperrmodus
* W-Lan / 3g bei Sperrung deaktivieren?

Geplante API
------------
### InfoProvider
InfoProvider werden als Bild/Text Kombination in der Infoleiste angezeigt.

![Info Provider](http://img830.imageshack.us/img830/9827/infobar.jpg "Info Provider")


Ungefähres Interface:

    interface IInfoProvider
    {
        // Das Intervall in welchem aktualisiert werden soll in Sekunden
        int UpdateInterval { get; }
        // Der aktuell angezeigte Text 
        string Label { get; }
        // Das zu verwendende Symbol
        Image Icon { get; }
        // Daten aktualisieren.
        void Update();
    }

### WidgetBase 
Die Basisklasse für das Erstellen neuer interaktiver Widgets. Jedes Widget ist eine Art Panel welches beliebige Steuerelemente beinhalten kann.

![Widget](http://img824.imageshack.us/img824/9334/widgetx.jpg "Widget")

### UnlockMode 
Diese Klasse ermöglicht es neue Entsperrmethoden zu entwickeln.  
2 Events Benachrichtigen ob ein Entsperrprozess erfolgreich war oder nicht:

    interface IUnlockMode
    {
        event UnlockEventHandler UnlockFailed;
        event UnlockEventHandler UnlockSuccess;
    }

### Lock/Unlock Notifier
Diese Klassen ermöglichen es neue Handler an die Lock/Unlock zu hängen. (bspw. um zu Player pausieren)

    interface ILockHandler 
    { 
        void OnLock(); 
    }

    interface IUnlockHandler 
    { 
        void OnUnlock(); 
    }
