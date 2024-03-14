document.addEventListener('DOMContentLoaded', function() {
    var loginForm = document.getElementById('loginForm');
    loginForm.addEventListener('submit', function(event) {
        event.preventDefault();

        var email = document.getElementById('emailInput').value;
        var password = document.getElementById('passwordInput').value;

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Auth/Login');
        xhr.setRequestHeader('Content-Type', 'application/json');

        xhr.onload = function() {
            if (xhr.status === 200) {
                console.log("pass");
                window.location.href = '/Home/Index';
            } else {
                var wrong = document.getElementById("wrong");
                wrong.style.display = 'flex';
                wrong.innerHTML = 'Incorrect email or password';
            }
        };

        xhr.onerror = function() {
            console.error('Request failed');
        };

        xhr.send(JSON.stringify({ Email: email, Password: password }));
    });
});
