﻿function addDetailLabel(ul, name, value){
	var num = parseInt(value);
	var li = document.createElement("ul");
	ul.appendChild(li);
	if(isNaN(num)){
		li.innerText = `${name}: ${value}`;
	} else {
		li.innerText = `${name}: ${value} (0x${num.toString(16)})`;
	}
	return li;
}
function setDetailPanel(element, detailView){
	const numberTypes = new Set(["UInt32", "Int32", "UInt16", "Int16", "Byte"]);
	var type = element.getAttribute("type");
	var isNumeric = numberTypes.has(type);
	var intValue = isNumeric ? parseInt(element.getAttribute("value")) : 0;
	detailView.innerHTML = "";
	var ul = document.createElement("ul");
	detailView.appendChild(ul);
	addDetailLabel(ul, "Name", element.getAttribute("name"));
	addDetailLabel(ul, "Type", element.getAttribute("type"));
	if(type == "Byte[]") {
		addDetailLabel(ul, "Value", `byte[${element.getAttribute("size")}]`);
	} else {
		addDetailLabel(ul, "Value", element.getAttribute("value"));
	}
	if(isNumeric){
		var ascii = "";
		for(var i = 0; i < 4; i++){
			var byte = (intValue >> (i * 8)) & 0xFF;
			ascii += byte >= 32 && byte <= 126 ? 
				String.fromCharCode(byte) : ".";
		}
		addDetailLabel(ul, "Ascii", ascii);
	}
	addDetailLabel(ul, "Size", element.getAttribute("size"));
	addDetailLabel(ul, "AbsStart", element.getAttribute("data-start"));
	addDetailLabel(ul, "AbsEnd", element.getAttribute("data-end"));
	addDetailLabel(ul, "RelStart", element.getAttribute("rel-start"));
	addDetailLabel(ul, "RelEnd", element.getAttribute("rel-end"));
	if(isNumeric){
		var offset = addDetailLabel(ul, "Offset", `${intValue}(0x${intValue.toString(16)})`);
		offset.onclick = function(){
			var hexByte = document.getElementById("b" + intValue);
			hexByte.click();
		}
	}
}
function isScrolledIntoView(el)
{
    var rect = el.getBoundingClientRect();
    var elemTop = rect.top;
    var elemBottom = rect.bottom;

    // Only completely visible elements return true:
    var isVisible = (elemTop >= 0) && (elemBottom <= window.innerHeight);
    // Partially visible elements return true:
    //isVisible = elemTop < window.innerHeight && elemBottom >= 0;
    return isVisible;
}
function setDetailPanelUnusedMemory(element, detailView) {
	var elements = [];
	var sibling = element;
	var intValue = 0;
	for(var i = 0; i < 4; i++){
		if (sibling !== null) {
			elements.push(sibling);
			intValue += parseInt(sibling.textContent, 16) << i * 8;
		} else {
			break;
		}
		sibling = sibling.nextSibling.nextSibling;
	}
	detailView.innerHTML = "";
	var ul = document.createElement("ul");
	detailView.appendChild(ul);
	addDetailLabel(ul, "Name", "UnreadMemory");
	addDetailLabel(ul, "Type", "UnreadMemory");
	addDetailLabel(ul, "Value", intValue.toString());

	var ascii = "";
	for (var i = 0; i < 4; i++) {
	var byte = (intValue >> (i * 8)) & 0xFF;
	ascii += byte >= 32 && byte <= 126 ?
		String.fromCharCode(byte) : ".";
	}
	addDetailLabel(ul, "Ascii", ascii);

	addDetailLabel(ul, "Size", 4);
	addDetailLabel(ul, "AbsStart", element.getAttribute("index"));
	addDetailLabel(ul, "AbsEnd", element.getAttribute("index") + 4);
	addDetailLabel(ul, "RelStart", element.getAttribute("index"));
	addDetailLabel(ul, "RelEnd", element.getAttribute("index") + 4);

	var offset = addDetailLabel(ul, "Offset", `${intValue}(0x${intValue.toString(16)})`);
	offset.onclick = function() {
	var hexByte = document.getElementById("b" + intValue);
	hexByte.click();
	}
}
window.onload = function(){
	console.log("hello");
	var detailView = document.getElementById("detailview");
	var bytes = document.getElementsByClassName("hex_byte");
	var ascii = document.getElementsByClassName("hex_ascii");
	var highlighted = [];
	var toggler = document.getElementsByClassName("caret");
	for (var i = 0; i < toggler.length; i++)
	{
		toggler[i].addEventListener("click", function() {
			this.parentElement.parentElement.querySelector(".nested").classList.toggle("active");
			this.classList.toggle("caret-down");
		});
	}
	var labels = document.getElementsByClassName("tree-label");
	for (var i = 0; i < labels.length; i++)
	{
		labels[i].addEventListener("click", function() {
			for(var j = 0; j < highlighted.length; j++){
				highlighted[j].classList.remove("highlighted");
			}
			highlighted = [];
			this.classList.add("highlighted");
			highlighted.push(this);
			var dataStart = parseInt(this.getAttribute("data-start"));
			var dataEnd = parseInt(this.getAttribute("data-end"));
			if(!isScrolledIntoView(bytes[dataStart])){
				bytes[dataStart].scrollIntoView();
			}
			for(var j = dataStart; j < dataEnd && j < bytes.length; j++){
				bytes[j].classList.add("highlighted");
				highlighted.push(bytes[j]);
				ascii[j].classList.add("highlighted");
				highlighted.push(ascii[j]);
			}
			setDetailPanel(this, detailView);
		});
	}
	for (var i = 0; i < bytes.length; i++)
	{
		bytes[i].addEventListener("click", function() {
			var memberId = this.getAttribute("member");
			var member = document.getElementById(memberId);
			if (member === null) {
				for(var j = 0; j < highlighted.length; j++){
					highlighted[j].classList.remove("highlighted");
				}
				setDetailPanelUnusedMemory(this, detailView);
				return;
			}
			if(!isScrolledIntoView(member)){
				member.scrollIntoView();
			}
			member.click();
			var parent = member;
			while(parent !== null && parent !== window){
				if(parent.classList.contains("nested") && !parent.classList.contains("active")){
					parent.classList.toggle("active");
					parent.previousElementSibling.firstElementChild.classList.toggle("caret-down");
				}
				parent = parent.parentElement;
			}
		});
	}
}