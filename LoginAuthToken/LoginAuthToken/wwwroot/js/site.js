// Helper to open a page with custom headers using fetch
window.openWithHeaders = async function (url, ip) {
    try {
        const response = await fetch(url, {
            method: "GET",
            headers: {
                "X-Client-IP": ip   // Send IP in custom HTTP header
            }
        });

        if (!response.ok) {
            alert("Could not authenticate with this IP");
            return;
        }

        // Replace current window content with the response (Project.razor)
        const data = await response.text();
        document.open();
        document.write(data);
        document.close();
    } catch (err) {
        console.error("Error in openWithHeaders:", err);
        alert("Connection error.");
    }
};
