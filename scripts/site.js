$(function () {
    prettyPrint();

    $('article').scrollspy({
        target: 'nav',
        offset: 20
    });

    $('a[href^=http]').attr({ target: '_blank' });
});