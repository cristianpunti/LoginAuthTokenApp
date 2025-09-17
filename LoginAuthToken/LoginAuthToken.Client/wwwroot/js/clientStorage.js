window.clientStorage = {
    setIp: function (ip) {
        console.log("[clientStorage] Guardando IP en localStorage:", ip);
        localStorage.setItem("clientIp", ip);
    },
    getIp: function () {
        const ip = localStorage.getItem("clientIp");
        console.log("[clientStorage] Leyendo IP desde localStorage:", ip);
        return ip;
    }
};
