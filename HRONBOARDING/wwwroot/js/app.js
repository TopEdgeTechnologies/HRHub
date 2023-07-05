let nextBtn = document.querySelector('#next-btn');
let companyName = document.querySelector('#CompanyName');
let companyTitle = document.querySelector('#company-title');
let staffName = document.querySelector('#ContactPerson');
// let staffDepartment = document.querySelector('#staffDepartment');
// let staffDesignation = document.querySelector('#staffDesignation');
// let staffPassword = document.querySelector('#staffPassword');
let staffNameOutput = document.querySelector('#staff-name-output');
let companyWebsite = document.querySelector('#WebUrl');
let formStep1 = document.querySelector('#form-step1');
let formStep2 = document.querySelector('#form-step2');
let formStep3 = document.querySelector('#form-step3');
let createAccountBtn = document.querySelector('#createAccountBtn');
let indicator = document.querySelectorAll('.indicator');
let emailIndicator = document.querySelector('.email-indicator');
let successIcon = document.querySelectorAll('.success-icon');
let emailSuccessIcon = document.querySelector('.email-success-icon');
let validationMessage = document.querySelectorAll('.validation-message');
let emailValidationMessage = document.querySelector('.email-validation-message');
let inputFields = document.querySelectorAll('.form-control');
let staffEmail = document.querySelector('#staffEmail');


// function for validating real time input 

function validateInput(inputElement, indicatorElement, validationMessageElement, successIconElement, inputFieldElement) {
    if (isNaN(inputElement.value)) {
        indicatorElement.style.visibility = "visible";
        validationMessageElement.textContent = "";
        indicatorElement.style.backgroundColor = "#27aa4e";
        successIconElement.classList.remove('fa-exclamation');
        successIconElement.classList.add('fa-check');
        inputFieldElement.style.border = "1px solid green";
    } else if (inputElement.value.trim() == "") {
        indicatorElement.style.background = "#da0830";
        indicatorElement.style.visibility = "visible";
        successIconElement.classList.remove('fa-check');
        successIconElement.classList.add('fa-exclamation');
        validationMessageElement.textContent = inputFieldElement.placeholder + " is required**";
        inputFieldElement.style.border = "1px solid red";
    } else {
        if (inputFieldElement.type != 'password') {
            validationMessageElement.textContent = "Numbers are not allowed**";
            successIconElement.classList.remove('fa-check');
            indicatorElement.style.visibility = "visible";
            successIconElement.classList.add('fa-exclamation');
            indicatorElement.style.background = "#da0830";
        }
        else {
            validationMessageElement.textContent = "";
            indicatorElement.style.backgroundColor = "#27aa4e";
            successIconElement.classList.remove('fa-exclamation');
            successIconElement.classList.add('fa-check');
            inputFieldElement.style.border = "1px solid green";
        }
    }
}
var regex = /[0-9]/gi;

//function for Email Validation

const validateEmail = () => {
    staffEmail.value.trim();
    /* for defining gloabal expressions we have to create a new object for that
    which defines the pattern and then match it globally*/
    const validRegex = new RegExp(/^[A-Za-z0-9_!#$%&'*+\/=?`{|}~^.-]+@[A-Za-z0-9.-]+$/, "gm");
    if (staffEmail.value == "") {
        emailValidationMessage.innerHTML = "Please provide a valid email address";
        emailValidationMessage.style.color = "#da0530";
        emailIndicator.style.background = "#da0830";
        emailIndicator.style.visibility = "visible";
        emailSuccessIcon.classList.remove('fa-check');
        emailSuccessIcon.classList.add('fa-exclamation');
        staffEmail.style.border = "1px solid red"
    }
    else if (validRegex.test(staffEmail.value)) {
        setTimeout(() => {
            emailValidationMessage.innerHTML = "";
        }, 3000)
        emailValidationMessage.innerHTML = "Valid Email address";
        emailValidationMessage.style.color = "#27aa4e";
        emailIndicator.style.visibility = "visible";
        emailIndicator.style.background = "#27aa4e";
        emailSuccessIcon.classList.remove('fa-exclamation');
        emailSuccessIcon.classList.add('fa-check');
        staffEmail.style.border = "1px solid green";
    }
    else {
        emailValidationMessage.innerHTML = "Please provide a valid email address";
        emailValidationMessage.style.color = "#da0530";
        staffEmail.style.border = "1px solid red"
        emailIndicator.style.visibility = "visible";
        emailIndicator.style.background = "#da0830";
        emailSuccessIcon.classList.remove('fa-check');
        emailSuccessIcon.classList.add('fa-exclamation');
    }
}

//   companyName.addEventListener('input', function() {
//     companyTitle.textContent = companyName.value;
//     validateInput(companyName, indicator[0], validationMessage[0], successIcon[0], inputFields[0]);
//   });

//   staffName.addEventListener('input', function() {
//     staffNameOutput.textContent = staffName.value;
//     validateInput(staffName, indicator[1], validationMessage[1], successIcon[1], inputFields[1]);
//   });
//   staffDepartment.addEventListener('input', function() {

//     validateInput(staffDepartment, indicator[2], validationMessage[2], successIcon[2], inputFields[2]);
//   });
//   staffDesignation.addEventListener('input', function() {
//     validateInput(staffDesignation, indicator[3], validationMessage[3], successIcon[3], inputFields[3]);
//   });
//   staffPassword.addEventListener('input', function(){
//     validateInput(staffPassword, indicator[5], validationMessage[5], successIcon[5], inputFields[5])
//   })

//handling input Events for each inputField using loop

inputFields.forEach(inputField => {
    inputField.addEventListener('input', function () {
        debugger
        const index = Array.from(document.querySelectorAll('.form-control')).indexOf(this);
        const indicatorElement = indicator[index];
        const validationMessageElement = validationMessage[index];
        const successIconElement = successIcon[index];
        const inputFieldElement = inputFields[index];


        if (this.id == 'CompanyName') {
            companyTitle.textContent = CompanyName.value;
        }

        if (this.id == 'ContactPerson') {
            staffNameOutput.textContent = ContactPerson.value;
        }
        validateInput(this, indicatorElement, validationMessageElement, successIconElement, inputFieldElement);
    });
});

staffEmail.addEventListener('input', validateEmail);
nextBtn.addEventListener('click', (event) => {
    event.preventDefault();
    if (!formStep1.classList.contains('d-none')) {
        if (companyName.value.trim() == "") {
            companyName.style.border = "1px solid red";
            document.querySelector('.companyNameValidationMessage').textContent = companyName.placeholder + " is required**";
            return;
        }
        formStep1.classList.add('d-none');
        formStep2.classList.remove('d-none');
    }
    else if (!formStep2.classList.contains('d-none')) {
        if (companyWebsite.value.trim() == "") {
            companyWebsite.style.border = "1px solid red";
            document.querySelector('.companyWebsiteValidationMessage').textContent = companyWebsite.placeholder + " is required**";
            return;
        }
        formStep2.classList.add('d-none');
        formStep3.classList.remove('d-none');
        nextBtn.classList.add('d-none');
        createAccountBtn.classList.remove('d-none');

    }
})
//function for ensuring all the fields are filled at the time of submission
const checkValidity = () => {
    
    var isValid = true;
    for (let i = 0; i < inputFields.length; i++) {
        
        if (indicator[i].style.visibility != "visible" && indicator[i].style.background != "#27aa4e") {
            inputFields[i].style.border = "1px solid red";
            validationMessage[i].textContent = inputFields[i].placeholder + " is required**";
            isValid = false;
        }
    }
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });
    if (isValid)
    {
        $("#myform").submit();   
        console.log("Data submit Success")
    }
}
createAccountBtn.addEventListener('click', checkValidity);































































//let nextBtn = document.querySelector('#next-btn');
//let companyName = document.querySelector('#CompanyName');
//let companyTitle = document.querySelector('#company-title');
//let staffName = document.querySelector('#ContactPerson');
//let staffDepartment = document.querySelector('#StaffDepartment');
//let staffDesignation = document.querySelector('#StaffDesignation');
//let staffPassword = document.querySelector('#UserPassword');
//let staffNameOutput = document.querySelector('#staff-name-output');
//let companyWebsite = document.querySelector('#companyWebsite');
//let formStep1 = document.querySelector('#form-step1');
//let formStep2 = document.querySelector('#form-step2');
//let createAccountBtn = document.querySelector('#createAccountBtn');
//let indicator = document.querySelectorAll('.indicator');
//let successIcon = document.querySelectorAll('.success-icon');
//let validationMessage = document.querySelectorAll('.validation-message');
//let inputFields = document.querySelectorAll('.form-control');
//let staffEmail = document.querySelector('#StaffEmail');


//// function for validating real time input 

//function validateInput(inputElement, indicatorElement, validationMessageElement, successIconElement, inputFieldElement) {
//    if (isNaN(inputElement.value)) {
//      indicatorElement.style.visibility = "visible";
//      validationMessageElement.textContent = "";
//      indicatorElement.style.backgroundColor = "#27aa4e";
//      successIconElement.classList.remove('fa-exclamation');
//      successIconElement.classList.add('fa-check');
//      inputFieldElement.style.border = "1px solid green";
//    } else if (inputElement.value.trim() == "") {
//      indicatorElement.style.background = "#da0830";
//      indicatorElement.style.visibility = "visible";
//      successIconElement.classList.remove('fa-check');
//      successIconElement.classList.add('fa-exclamation');
//      validationMessageElement.textContent = inputFieldElement.placeholder + " is required**";
//      inputFieldElement.style.border = "1px solid red";
//    } else {
//      validationMessageElement.textContent = "Numbers are not allowed**";
//      successIconElement.classList.remove('fa-check');
//      indicatorElement.style.visibility = "visible";
//      successIconElement.classList.add('fa-exclamation');
//      indicatorElement.style.background = "#da0830";
//    }
//  }
//  var regex = /[0-9]/gi;

//  //function for Email Validation

//  const validateEmail = () => {
//        staffEmail.value.trim();
//        /* for defining gloabal expressions we have to create a new object for that
//        which defines the pattern and then match it globally*/
//        const validRegex = new RegExp(/^[A-Za-z0-9_!#$%&'*+\/=?`{|}~^.-]+@[A-Za-z0-9.-]+$/, "gm");
//        if (staffEmail.value == "") {
//          validationMessage[4].innerHTML = "Please provide a valid email address";
//          validationMessage[4].style.color = "#da0530";
//          indicator[4].style.background = "#da0830";
//          indicator[4].style.visibility = "visible";
//          successIcon[4].classList.remove('fa-check');
//          successIcon[4].classList.add('fa-exclamation');
//          inputFields[4].style.border = "1px solid red"
//        }
//        else if (validRegex.test(staffEmail.value)) {
//          setTimeout(()=>{
//            validationMessage[4].innerHTML = "";
//          },3000)
//          validationMessage[4].innerHTML = "Valid Email address";
//          validationMessage[4].style.color = "#27aa4e";
//          indicator[4].style.visibility = "visible";
//          indicator[4].style.background = "#27aa4e";
//          successIcon[4].classList.remove('fa-exclamation');
//          successIcon[4].classList.add('fa-check');
//          inputFields[4].style.border = "1px solid #dee2e6";
//          inputFields[4].style.border = "1px solid green"
//        }
//        else {
//          validationMessage[4].innerHTML = "Please provide a valid email address";
//          validationMessage[4].style.color = "#da0530";
//          inputFields[4].style.border = "1px solid red"
//          indicator[4].style.visibility = "visible";
//          indicator[4].style.background = "#da0830";
//          successIcon[4].classList.remove('fa-check');
//          successIcon[4].classList.add('fa-exclamation');
//        }
//  }
  
//  companyName.addEventListener('input', function() {
//    companyTitle.textContent = companyName.value;
//    validateInput(companyName, indicator[0], validationMessage[0], successIcon[0], inputFields[0]);
//  });
  
//  staffName.addEventListener('input', function() {
//    staffNameOutput.textContent = staffName.value;
//    validateInput(staffName, indicator[1], validationMessage[1], successIcon[1], inputFields[1]);
//  });
//  staffDepartment.addEventListener('input', function() {
    
//    validateInput(staffDepartment, indicator[2], validationMessage[2], successIcon[2], inputFields[2]);
//  });
//  staffDesignation.addEventListener('input', function() {
//    validateInput(staffDesignation, indicator[3], validationMessage[3], successIcon[3], inputFields[3]);
//  });
//  staffPassword.addEventListener('input', function(){
//    validateInput(staffPassword, indicator[5], validationMessage[5], successIcon[5], inputFields[5])
//  })

//  staffEmail.addEventListener('input', validateEmail);


//staffName.addEventListener('input', function () {
//    staffNameOutput.textContent = staffName.value;
//});
//nextBtn.addEventListener('click', (event) => {
//    event.preventDefault();
//    // formStep1.classList.remove('active');
//    // formStep1.classList.add('d-none');
//    // formStep2.classList.remove('d-none');
//    // formStep2.classList.add('active');

//    if (!formStep1.classList.contains('d-none')) {
//        if (companyName.value.trim() == "") {
//            // alert('fill the field');
//            companyName.style.border = "1px solid red";
//            validationMessage[0].textContent = companyName.placeholder + " is required**";
//            return;
//        }
//        formStep1.classList.add('d-none');
//        formStep2.classList.remove('d-none');
//        createAccountBtn.classList.remove('d-none');
//        nextBtn.classList.add('d-none');

//    }
//})

////function for ensuring all the fields are filled at the time of submission
//const checkValidity = (e) =>{
//    e.preventDefault();
//    let isValid = true;
//    for(let i = 0; i <inputFields.length ; i++){
//        if(indicator[i].style.visibility != "visible" && indicator[i].style.background != "#27aa4e"){
//            inputFields[i].style.border = "1px solid red";
//            validationMessage[i].textContent =  inputFields[i].placeholder + " is required**"
//        }
//    }
//    if (isValid) {
//        console.log("Data post success")
//    }
//}
//createAccountBtn.addEventListener('click', checkValidity);