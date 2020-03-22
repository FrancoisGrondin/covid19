TODO APP
========

Post-Prototype
    - Connection to MS AppCenter
    - Localize app


NOTES SERVEUR
=============

    - La connection doit utiliser le protocol SSL (donc https)

    - Les champs Altitude, Speed et Course ne sont pas garanti d'avoir une valeur. Il faudra gérer ces exceptions dans l'app ou dans le serveur 


NOTES DISTRIBUTION APP
======================

    - Il faudra avoir un partenariat avec une entitée oeuvrant officiellement en santé (ex. CHUM) pour que l'AppStore et le PlayStore acceptent une telle app. 


DIVERS
======

    - Besoin d'avoir les UDID des iPhones des testeurs pour pouvoir permettre l'installation de l'app sur leurs téléphones. (https://www.imore.com/how-find-your-iphones-serial-number-udid-or-other-information) 

    - Altitude : le GPS estime mal l'altitude. On peut le prendre, mais je ne vois pas trop ce qu'on fera avec.
 
    - Précision : le GPS estime un niveau de confiance habituellement exprimé par le rayon (en mètres) d'un cercle autour du point estimé qui a 85% de chance de contenir la vraie position. Ça prendrait ça.
 
    - Pour éviter d'avoir trop données dans les logs, l'App doit faire un travail minimal pour retirer les positions répétitive.
 
    - François : ligne 29 def do_POST(self): il faut vraiment traiter une liste de points en batch plutôt que une à une. Ta fonction ouvre un fichier, écrit et le ferme. Ça sera coûteux, car ça va générer plusieurs I/O sur le disque/ssd (les blocs du fichier + les métadonnées comme lastmodif, etc.). Bref, l'app doit envoyer par exemple au 5-15-60 minutes un array et cet array deva être écrit séquentiellement sur disque, avec un seul flush à la toute fin (à la fermeture du fichier).
