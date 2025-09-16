window.openWithIpHeader = async function (url, ip) {
    const response = await fetch(url, {
        method: "GET",
        headers: {
            "X-Client-IP": ip
        }
    });

    const data = await response.text();
    const newWindow = window.open();
    newWindow.document.write(data);
};

