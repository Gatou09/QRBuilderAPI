let token = "";

function login() {
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    fetch("/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
    })
        .then(res => res.json())
        .then(data => {
            if (data.token) {
                token = data.token;
                document.getElementById("generate-section").style.display = "block";
                alert("Connecté !");
            } else {
                alert("Erreur de connexion");
            }
        });
}

function generate() {
    const value = document.getElementById("value").value;

    fetch("/qr/generate", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            type: "url",
            value: value,
            color: "#000000",
            backgroundColor: "#ffffff"
        })
    })
        .then(res => res.json())
        .then(data => {
            document.getElementById("result").innerHTML = `
        <p>QR généré :</p>
        <img src="${data.url}" alt="QR Code" />
      `;
        });
}
