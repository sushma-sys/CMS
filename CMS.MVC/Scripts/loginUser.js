function userValid() {
    var isFormValid = true;
    var EmailId, Password, emailExp;
    EmailId = document.getElementById("EmailId").value;
    Password = document.getElementById("Password").value;
    emailExp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([com\co\.\in])+$/;
    if (EmailId == "".trim()) {
        document.getElementById("mail").innerText = "*Enter EmailId";
        isFormValid = false;
    }
    else {
        document.getElementById("mail").innerText = "";
    }
    if (Password == "".trim()) {
        document.getElementById("pass").innerText = "*Enter Password";
        isFormValid = false;
    }
    else {
        document.getElementById("pass").innerText = "";
    }
    if (EmailId != "") {
        if (!EmailId.match(emailExp)) {
            document.getElementById("mail").innerText = "*Invalid mail";
            isFormValid = false;
        }
    }
    if (!isFormValid) {
        return false;
    }
    else {
        //$("#formId").submit();
        document.getElementById("formId").submit();
    }
}