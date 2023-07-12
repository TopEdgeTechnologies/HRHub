
function generateCaptcha() {
    var captcha = Math.random().toString(36).slice(2, 8);
    $('#captcha-container').text(captcha);
}

function storeLoginAttempt() {

    // Increment the login attempt counter
    var loginCounter = localStorage.getItem('loginCounter');
    loginCounter = loginCounter ? parseInt(loginCounter) + 1 : 1;
    localStorage.setItem('loginCounter', loginCounter);


}
