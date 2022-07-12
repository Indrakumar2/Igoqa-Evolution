import React, { Component, Fragment } from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
// ToDo - Rich Editor Css
import './quill.css';
import { fieldLengthConstants } from '../../../constants/fieldLengthConstants';
import { modalMessageConstant, modalTitleConstant } from '../../../constants/modalConstants';
import CustomModal from '../customModal';
import IntertekToaster  from '../../baseComponents/intertekToaster';

class QuillComponent extends Component {
    constructor(props) {
        super(props);
        this.state = { text: props.text };
        this.handleChange = this.handleChange.bind(this);
        this.modules = this.modules.bind(this);
        this.options = [];
        this.placeholdersArray = [];
        this.state = {
            isOpen: false
        };
        this.quillRef = null;      // Quill instance
        this.reactQuillRef = null;
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        
    }

    modules() {
       const mode = this.props.readOnly;
        return {
           
            toolbar: {
                container: [
                    [
                        {
                            'header': [
                                1,
                                2,
                                3,
                                false
                            ]
                        }
                    ],
                    [
                        'italic',
                        'underline'
                    ],
                    [
                        { 'list': 'ordered' }, { 'list': 'bullet' }
                    ],
                    [
                        'link'
                    ],
                    [
                        { 'editorPlaceholder': this.options }
                    ],
                ],
               
                  handlers: {
                    "editorPlaceholder": function (value) {
                    if(!mode)
                    {                       
                      if (value) {
                            const cursorPosition = this.quill.getSelection().index;
                            this.quill.insertText(cursorPosition, '@' + value + '@ ');
                            this.quill.setSelection(cursorPosition + (value.length + 3));
                        }
                    }
                },     
                    "link": function (value)
                    {
                        //D691 issue-2
                        if(!mode) {
                            const cursorPosition = this.quill.getSelection().length;
                            if (value && cursorPosition) {
                                const href = prompt('Enter the URL');
                                if(href && (href.includes("http://") || href.includes("https://")))
                                {
                                    this.quill.format('link', href);
                                }else if(href!=null){
                                    IntertekToaster("Invalid Url Ex.(http://xxx.com/)", "warningToast");
                                }                            
                            } else {
                                this.quill.format('link', false);
                            }    
                        }                    
                    }
            
        }
            }
        };
    }

    formats() {
        return [
            'header', 'italic', 'underline', 'blockquote',
            'list', 'bullet', 'indent',
            'link'
        ];
    }
    componentDidMount() {
        this.attachQuillRefs();
      }
    
      componentDidUpdate() {
        this.attachQuillRefs();
      }

      attachQuillRefs = () => {
          if(this.reactQuillRef){  //Added for Sanity Defect 112
            if (typeof this.reactQuillRef.getEditor !== 'function') return;
            this.quillRef = this.reactQuillRef.getEditor();
          }
      }

    handleChange(e) {
        // if (e.length > 4000) {
        //     const confirmationObject = {
        //         title: modalTitleConstant.CONFIRMATION,
        //         message: modalMessageConstant.EMAIL_TEMPLATE_TEXT_LIMIT,
        //         type: "confirm",
        //         modalClassName: "warningToast",
        //         buttons: [
        //             {
        //                 buttonName: "Close",
        //                 onClickHandler: this.confirmationRejectHandler,
        //                 className: "modal-close m-1 btn-small"
        //             }
        //         ]
        //     };
        //     this.props.actions.DisplayModal(confirmationObject);
        // }
        const limit =fieldLengthConstants.company.CompanyEmail.COMPANY_EMAIL_TEMPLATE ;
        const quill = this.quillRef;
        quill && quill.on('text-change', function (delta, old, source) {
          if (quill.getLength() > limit) {
           quill.deleteText(limit, quill.getLength());
          }
        });
        //Added for Sanity Defect 112   -Start
        if(e === "<p><br></p>"){
            e="";
        }
        //Added for Sanity Defect 112   -End
        this.props.actions.UpdateCompanyEmailTemplate({ "emailType": this.props.templateValue, "template": e });
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    render() {
        const placeholders = this.props.editorPlaceholders;
        // const ddlList = this.props.editorPlaceholders;  //  Not used
        this.options = [];
        for (const item in placeholders) {
            this.options.push(
                placeholders[item].name
            );
        }
        // Code to set the options and label for custom select box in the rich-editor #issue with the plugin
        // TODO - Remove this code once the issue is resolved with the plugin
        setTimeout(() => {
            // Setting the label for select box            
            const selectBoxes = document.getElementsByClassName('ql-picker-label');
            if (selectBoxes[1]) {
                let label = selectBoxes[1].innerHTML;
                if (!label.includes("Select")) {
                    label = 'Select' + label;
                    document.getElementsByClassName('ql-picker-label')[1].innerHTML = label;
                }
            }
            // Setting the labels for options
            const pickerItems = document.getElementsByClassName('ql-picker-item');
            for (const item in pickerItems) {
                if (item > 3) {                    
                    const dataValue = pickerItems[item].getAttribute('data-value');
                    const filterValue = this.props.editorPlaceholders.filter(x => x.name == dataValue)[0];
                    if (filterValue != null) {
                        pickerItems[item].setAttribute('data-value', filterValue.name);
                        pickerItems[item].innerHTML = filterValue.displayName;
                    }
                }
            }
        }, 0);
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };

        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <ReactQuill
                     ref={(el) => { this.reactQuillRef = el; }}
                    theme='snow'
                    value={this.props.templateValue ? this.props.text ? this.props.text : '' : ''}
                    onChange={this.handleChange}
                    modules={this.modules()}
                    formats={this.formats()}
                    readOnly= { this.props.readOnly ? true: this.props.templateValue ==='' ? true : false }
                />
            </Fragment>
        );
    }
}

export default QuillComponent;