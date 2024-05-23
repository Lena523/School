document.querySelector('.burger').onclick = function(){
    document.querySelector('.menu ul').classList.add('mob-menu');
}

document.querySelector('#close-btn').onclick = function(){
    document.querySelector('.menu ul').classList.remove('mob-menu');
}

document.querySelector('.person').onclick = function(){
    document.querySelector('.registration').classList.add('reg-menu');
}

document.querySelector('.cross').onclick = function(){
    document.querySelector('.registration').classList.remove('reg-menu');
}


function switchParagraph1(){
    var btn1 = document.querySelector('#plus1');
    var info1 = document.querySelector('#info1');
    btn1.classList.toggle('close');
    info1.classList.toggle('info-hide');
}

document.querySelector('#plus1').onclick = switchParagraph1;

function switchParagraph2(){
    var btn2 = document.querySelector('#plus2');
    var info2 = document.querySelector('#info2')
    btn2.classList.toggle('close');
    info2.classList.toggle('info-hide');
}

document.querySelector('#plus2').onclick = switchParagraph2;

function switchParagraph3(){
    var btn3 = document.querySelector('#plus3');
    var info3 = document.querySelector('#info3')
    btn3.classList.toggle('close');
    info3.classList.toggle('info-hide');
}

document.querySelector('#plus3').onclick = switchParagraph3;