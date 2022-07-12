import React,{ Component,Fragment } from 'react';
//convert the component in stateless component
import Draggable from 'react-draggable'; // The default

const customModal = (props) =>{
    const { modalData }=props;
    const { buttons,isOpen, title, modalClassName,message, isSetModalHeight, isMaxWidth } = modalData;
    const maxWidthSet = isMaxWidth ? 700 : 400;
    const resultantMessage = message && message.split('\n').map(function(item, key) {
        return (
          <span key={key}>
            {item}
            <br/>
          </span>
        );
    });
    return (
        <Fragment>
            <Draggable>
        <div className={isOpen?"modal show customModal":""} style={isOpen?{ display:'block', maxWidth:maxWidthSet }:{ display:'none' }}>
         <div id="confirmation_Modal" className="confimationModal">
            <div className="modal-content p-0">
                <div className={modalClassName+' pl-2 mb-2'}>
                    <h5 className="bold">{title}</h5>
                </div>
                <p className={isSetModalHeight? "p-2 setConfirmationBodyText" : "p-2 confimationBodyText confirmationModalOverlay"}>{resultantMessage}</p>
            </div>
            <div className="modal-footer">
            {
                buttons.length>0?buttons.map((value,key)=>(
                    <button key={key} className={value.className} onClick={value.onClickHandler}>{value.buttonName}</button>
                )):''
            }
            </div>
        </div>
        </div>
        </Draggable>
        {isOpen && <div className="customModalOverlay"></div> }
      </Fragment>
    );
};

export default customModal;