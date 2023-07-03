let nextBtn = document.querySelector('#next-btn');
let companyName = document.querySelector('#CompanyName');
let companyTitle = document.querySelector('#company-title');
let staffName = document.querySelector('#ContactPerson');
let staffNameOutput = document.querySelector('#staff-name-output');
let staffDepartment = document.querySelector('#StaffDepartment');
let formStep1 = document.querySelector('#form-step1');
let formStep2 = document.querySelector('#form-step2');
let formStep3 = document.querySelector('#form-step3');
let createPortalBtn = document.querySelector('#create-portal-btn');
let indicator = document.querySelectorAll('.indicator');
let successIcon = document.querySelectorAll('.success-icon');
let validationMessage = document.querySelectorAll('.validation-message');
let inputFields = document.querySelectorAll('.form-control')


companyName.addEventListener('input', function () {
    companyTitle.textContent = companyName.value;
        if (isNaN(companyName.value)) {
          indicator[0].style.visibility = "visible";
          validationMessage[0].textContent = "";
          indicator[0].style.backgroundColor = "#27aa4e";
          successIcon[0].classList.remove('fa-exclamation');
          successIcon[0].classList.add('fa-check');
          inputFields[0].style.border = "1px solid green";
        }
        else if (companyName.value.trim() == "") {
          indicator[0].style.background = "#da0830";
          indicator[0].style.visibility = "visible";
          successIcon[0].classList.remove('fa-check');
          successIcon[0].classList.add('fa-exclamation');
          validationMessage[0].textContent = "Organization Name is required**";
          inputFields[0].style.border = "1px solid red";
        }
        else{
          validationMessage[0].textContent = "Numbers are not allowed**";
          successIcon[0].classList.remove('fa-check');
          indicator[0].style.visibility = "visible";
          successIcon[0].classList.add('fa-exclamation');
          indicator[0].style.background = "#da0830";
        }
});

staffName.addEventListener('input', function () {
    staffNameOutput.textContent = staffName.value;
});
nextBtn.addEventListener('click', (event) => {
    event.preventDefault();
    // formStep1.classList.remove('active');
    // formStep1.classList.add('d-none');
    // formStep2.classList.remove('d-none');
    // formStep2.classList.add('active');

    if (!formStep1.classList.contains('d-none')) {

        if (companyName.value.trim() == "") {
            alert('fill the field');
            return;
        }
        formStep1.classList.add('d-none');
        formStep2.classList.remove('d-none');
    }
    else if (!formStep2.classList.contains('d-none')) {

        if (staffName.value.trim() == "") {
            alert('fill the field');
            return;
        }
        formStep2.classList.add('d-none');
        formStep3.classList.remove('d-none');
        createPortalBtn.classList.remove('d-none');
        nextBtn.classList.add('d-none');
    }

})