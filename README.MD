# Endo'travail - Le Serious Game sur l'Endométriose au travail (Unity)

## Déploiement

``1. Vérifier que les préfabs des scènes addressables correspondent bien à celles dans le dossier Ressources.``

``2. Créer un groupe addressables par scène addressables. Attribuer un label pour tous les assets de chaque groupes (sauf la scène). Mettre à jour la liste des addresses de scène et le code correspondant au chargement.``

``3. Bien vider le cache des addressables. Créer un groupe Duplicate Assets Isolation depuis l'interface 'Windows > Addressables > Analyze.'``

``4. Compiler les bundles addressables depuis l'interface 'Windows > Addressables > Groups.'``

``5. Build le projet en WebGL. Zip le Build. Transfère le build sur le serveur.'``

    !!! Attention à bien désactiver le CDN ou le mettre en bypass si il y a plusieurs versions sur le serveur !!!

    !!! En cas de bug à la décompression d'un bundle sur Webgl vérifier qu'un asset n'est pas partagé entre scène non addressable et scène addressable (ex: theme.mp3 sur la Home et sur End ?) !!!
