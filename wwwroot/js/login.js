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
                window.location.href = '/Home/Index';
            },
            error: function(xhr, status, error) {
                $('#message').html('Login failed. Please try again.');
            }
        });
    });
});
