$(document).ready(function() {
    $('#loginForm').submit(function(event) {
        event.preventDefault();
        
        var email = $('#emailInput').val();
        var password = $('#passwordInput').val();

        $.ajax({
            type: 'POST',
            url: '/Auth/Login',
            data: {
                Email: email,
                Password: password
            },
            success: function(response) {
                    console.log("pass");
                    window.location.href = '/Home/Index';
            },
            error: function(xhr, status, error) {
                var wrong = document.getElementById("wrong");
                wrong.style.display = 'flex';
                $('#wrong').html('Login failed. Please try again.');
            }
        });
    });
});
