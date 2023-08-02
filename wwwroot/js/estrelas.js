function criaEstrela() {
    var estrela = document.createElement("div");
    estrela.style.width = "2px";
    estrela.style.height = "2px";
    estrela.style.background = "white";
    estrela.style.position = "absolute";
    estrela.style.left = Math.random() * window.innerWidth + 'px';
    estrela.style.top = Math.random() * window.innerHeight + 'px';
    estrela.style.borderRadius = "50%";
    estrela.className = "estrela"; // Adicionando uma classe para a estrela

    return estrela;
}

function adicionaEstrelasAoCeu() {
    var ceu = document.getElementById('estrelas');

    for (var i = 0; i < 300; i++) {
        var estrela = criaEstrela();
        ceu.appendChild(estrela);
    }
}

function fazEstrelasCintilarem() {
    var estrelas = document.getElementsByClassName('estrela');

    for (var i = 0; i < estrelas.length; i++) {
        var opacidade = Math.random();
        estrelas[i].style.opacity = opacidade;
    }
}

adicionaEstrelasAoCeu();

setInterval(fazEstrelasCintilarem, 1300);