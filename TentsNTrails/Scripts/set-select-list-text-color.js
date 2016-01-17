function setSelectListTextColor(selectId) {
    var select = document.getElementById(selectId);
    var option = select.options[select.selectedIndex];
    select.style.color = option.value.length === 0 ? '#999999' : '#000000';
}