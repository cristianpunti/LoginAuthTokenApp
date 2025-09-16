// wwwroot/js/trianglifyHelper.js
window.trianglifyHelper = {
    generarCanvas: function (options, elementId) {
        const pattern = trianglify(options); // trianglify viene de trianglify.min.js
        const el = document.getElementById(elementId);
        if (!el) {
            console.error("Elemento no encontrado:", elementId);
            return;
        }

        // Limpiar contenido previo y añadir canvas
        el.innerHTML = '';
        const canvas = pattern.toCanvas();
        canvas.id = "trianglify-overlay";
        el.appendChild(canvas);
    },

    generarDataURL: function (options) {
        const pattern = trianglify(options);
        const canvas = pattern.toCanvas();
        return canvas.toDataURL();
    }
};
