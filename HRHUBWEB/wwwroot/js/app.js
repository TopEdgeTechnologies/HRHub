// Define a click event handler for the button
document.getElementById("btnShowName").addEventListener("click", function () {
    // Get the values from the textboxes
    const firstName = document.getElementById("txtFirstName").value;
    const lastName = document.getElementById("txtLastName").value;
    // Display the names
    document.getElementById("txt_fname").innerText = firstName;
    document.getElementById("txt_lastName").innerText = lastName;
});
//# sourceMappingURL=app.js.map


