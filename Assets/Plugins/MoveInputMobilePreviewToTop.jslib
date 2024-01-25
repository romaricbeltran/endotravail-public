var plugin = {
    MoveInputMobilePreviewToTop: function () {
        let isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);

        if (isMobile) {
            setTimeout(function () {
                let lastElement = document.body.lastElementChild;
                if (lastElement) {
                    var input = lastElement.getElementsByTagName('input');
                    var textarea = lastElement.getElementsByTagName('textarea');

                    if (input.length > 0 || textarea.length > 0) {
                        lastElement.style.bottom = "auto";
                        lastElement.style.top = "0";
                    }
                } else {
                    console.log("Le dernier élément du body ne contient pas d'élément input ou textarea.");
                }
            }, 300);
        } else {
            console.log("Non exécuté sur un dispositif mobile.");
        }
    },
};

mergeInto(LibraryManager.library, plugin);
