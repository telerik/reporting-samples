; function themeSwitcher(switcher, commonCss, skinCss) {
    var
        skins = [
            { value: "blueopal", id: "blueopal", name: "Blue Opal" },
            { value: "black", id: "black", name: "Black" },
            { value: "bootstrap", id: "bootstrap", name: "Bootstrap" },
            { value: "default", id: "default", name: "Default" },
            { value: "fiori", id: "fiori", name: "Fiori" },
            { value: "flat", id: "flat", name: "Flat" },
            { value: "highcontrast", id: "highcontrast", name: "High Contrast" },
            { value: "material", id: "material", name: "Material" },
            { value: "materialblack", id: "materialblack", name: "Material Black" },
            { value: "metro", id: "metro", name: "Metro" },
            { value: "metroblack", id: "metroblack", name: "Metro Black" },
            { value: "moonlight", id: "moonlight", name: "Moonlight" },
            { value: "office365", id: "office365", name: "Office 365" },
            { value: "silver", id: "silver", name: "Silver" },
            { value: "uniform", id: "uniform", name: "Uniform" },
            { value: "nova", id: "nova", name: "Nova" },
        ];

    var $switcher = $(switcher),
        $commonCss = $(commonCss),
        $skinCss = $(skinCss),
        specialSkins = ["nova", "bootstrap", "fiori", "material", "materialblack", "office365"],
        skinName = getParameterByName("skinName"),
        path = "https://kendo.cdn.telerik.com/2018.2.620/styles/",
        i;

    $switcher.empty();

    for (i = 0; i < skins.length; i++) {
        $switcher
            .append(
                $('<option>', {
                    value: skins[i].value,
                    text: skins[i].name,
                    id: skins[i].id
                }));
    }

    if (skinName && skinName !== "") {
        var commonSkinName = specialSkins.indexOf(skinName) !== -1 ? ("-" + skinName) : "";
        var commonSkin = path + "kendo.common" + commonSkinName.replace("materialblack", "material") + ".min.css";
        var skin = path + "/kendo." + skinName + ".min.css";

        $commonCss.attr('href', commonSkin);
        $skinCss.attr('href', skin);

        //$switcher.get(0).options[skinName].selected = "selected";
        for (i = 0; i < $switcher.get(0).options.length; i++) {
            if ($switcher.get(0).options[i].value === skinName) {
                $switcher.get(0).options[i].selected = "selected";
            }
        }
    }

    $switcher.change(function () {
        var id = $("select option:selected").attr("id");

        insertParam("skinName", id);

        var commonSkinName = specialSkins.indexOf(id) !== -1 ? ("-" + id) : "";
        var commonSkin = path + "kendo.common" + commonSkinName.replace("materialblack", "material") + ".min.css";
        var skin = path + "/kendo." + id + ".min.css";

        $commonCss.attr('href', commonSkin);
        $skinCss.attr('href', skin);
    });

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    };

    function insertParam(key, value) {
        key = encodeURI(key);
        value = encodeURI(value);

        var kvp = document.location.search.substr(1).split('&');

        var i = kvp.length;
        var x;
        while (i--) {
            x = kvp[i].split('=');

            if (x[0] === key) {
                x[1] = value;
                kvp[i] = x.join('=');
                break;
            }
        }

        if (i < 0) { kvp[kvp.length] = [key, value].join('='); }
         document.location.search = kvp.join('&');    };
};