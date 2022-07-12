import React, { Component,Fragment } from 'react';
import Draggable from 'react-draggable'; // The default
import PropTypes from 'prop-types';

class Modal extends Component {      
    render() {   
        const { title, children, isButton, buttonName,isWindowRestoreIcon,fixedComponent,isFxiedComponent, isShowButtonHeader, buttons,isShowModal,modalClass,modalContentClass,modalId,formId,onSubmit,autocomplete, titleClassName,overlayClass,isCloseIcon,isDefaultColseIcon,isDisableDrag } = this.props;    
        return (
            <Fragment>
               <Draggable handle=".handle" position={ fixedComponent } disabled={isDisableDrag}>
                <div  id={modalId} className={isShowModal ? 'modal modelShow '+ modalClass: 'modal'}>
                <form id={formId} onSubmit={onSubmit} autoComplete={autocomplete} >
                {title && <h6 className={titleClassName? titleClassName + " handle" :'handle'}>{title}
                { isShowButtonHeader && <div className="col right">
                    { buttons.map((button,i)=>{
                       return(                       
                        (button.showbtn && isCloseIcon) || button.windowRestoreIcon ? <i class={"zmdi " + button.btnClass} onClick={button.action}></i> : <button type={button.type || null} disabled={button.disabled || null } id={button.btnID || null} key={i} className={button.btnClass} onClick={button.action} >{button.name}</button>
                        );                    
                        })}
                    </div>
                    } 
                    { !isShowButtonHeader && (isDefaultColseIcon && !isCloseIcon) && <div className="col s1 iconWidth right pointer">
                    { buttons.map((button,i)=>{
                       return(                       
                            button.name === 'Cancel' && <i class={"zmdi zmdi-close"} onClick={button.action}></i> 
                        );                    
                        })}
                    </div>
                    }               
                </h6>}
                <span class="boldBorder"></span>
                { isFxiedComponent &&
                    <div className="col s12">
                      { fixedComponent }
                    </div>  
                }

                <div className={'modal-content '+ modalContentClass}>             
               
                    {children}
                </div>
              
                { !isShowButtonHeader && <div className="modal-footer">
                   {buttons.map((button,i)=>{
                       return( 
                        button.showbtn && <button type={button.type || null} disabled={button.disabled || null } id={button.btnID || null} key={i} className={button.btnClass} onClick={button.action} >{button.name}</button>
                       );                    
                   })}
                </div> }
                </form>
                </div>
                </Draggable>                
                { isShowModal && <div className={"modalOverlay " +overlayClass}></div> }

            </Fragment>
        );
   }
}
export default Modal;
Modal.propTypes = {
    modalClass: PropTypes.string,
    overlayClass:PropTypes.string,
    modalContentClass: PropTypes.string,
    buttons:PropTypes.array,
    isDefaultColseIcon:PropTypes.bool
};
Modal.defaultProps = {
    modalClass: '',
    overlayClass:'',
    modalContentClass: '',
    autocomplete:'off',
    buttons:[] ,
    disabled:false ,
    isCloseIcon:false,  
    isDefaultColseIcon:true,
    isShowButtonHeader:false,
    
}; 