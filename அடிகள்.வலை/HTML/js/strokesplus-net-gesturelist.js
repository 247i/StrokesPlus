/********************************************************************************************
    Gestures - Utility Class
*********************************************************************************************/

class Gestures {

    constructor() {
        this.NewGestureName = "";
    }

    DrawArrowBorder(ctx, fromx, fromy, tox, toy, r, penBorder) {
        var x_center = tox;
        var y_center = toy;
        
        var angle;
        var x;
        var y;
        
        ctx.lineWidth = 1;
        ctx.strokeStyle = penBorder;
        ctx.beginPath();
        
        angle = Math.atan2(toy-fromy,tox-fromx)
        
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
    
        ctx.moveTo(x, y);
        
        angle += (1/3)*(2*Math.PI)
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
        
        ctx.lineTo(x, y);
        
        angle += (1/3)*(2*Math.PI)
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
        
        ctx.lineTo(x, y);
        ctx.closePath();
        ctx.stroke();    
    }
    
    DrawArrowTip(ctx, fromx, fromy, tox, toy, r, arrowColor){
        var x_center = tox;
        var y_center = toy;
        
        var angle;
        var x;
        var y;
    
        ctx.beginPath();
        
        angle = Math.atan2(toy-fromy,tox-fromx)
        
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
    
        ctx.moveTo(x, y);
        
        angle += (1/3)*(2*Math.PI)
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
        
        ctx.lineTo(x, y);
        
        angle += (1/3)*(2*Math.PI)
        x = r*Math.cos(angle) + x_center;
        y = r*Math.sin(angle) + y_center;
        
        ctx.lineTo(x, y);
        ctx.closePath();
        ctx.fillStyle = arrowColor;
        ctx.fill();
    }
    
    DrawToCanvas(gesture, targetElement, penWidth, penColor, regions, borderColor, regionColor, penBorder, arrowColor, width, height) {
        var canvas = document.getElementById(targetElement);
        if(!canvas) {
            canvas = targetElement;
        }
        var ctx = canvas.getContext('2d');
        let drawBorder = true;
    
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    
        if(!penColor) {
            penColor = colorNetToHex(appSettings.PenColor);
        }
        if(!arrowColor) {
            arrowColor = penColor;
        }
        if(!width) {
            width = canvas.width;
        }
        if(!height) {
            height = canvas.height;
        }
        if(!penWidth) {
            penWidth = Math.floor((width * 0.6) * 0.1);
        }
        if(!borderColor) {
            borderColor = '#000000';
        }    
        if(!regionColor) {
            regionColor = '#888888';
        }
        if(!penBorder) {
            penBorder = '#FFFFFF';
        } 
    
        let actionActive = $(canvas).data('active');
        if(actionActive === true || actionActive == null || actionActive === "") {
            $(canvas).css('opacity', '1.0');
            $(canvas).css('background-color', '');
        } else {
            $(canvas).css('opacity', '0.5');
            $(canvas).css('background-color', '#f2f2f2');
        }
    
        //Test luminance, use dark gray stroke color instead if pen color too light
        let rgb = colorHexToRGB(penColor);
        let lum = 0.2126*rgb.r + 0.7152*rgb.g + 0.0722*rgb.b;
        if(lum > 180) {
            penColor = $(canvas).data('usesecondary') === true ? '#777777' : '#38A9FF';
            arrowColor = penColor;
    
            if(!appSettings.SettingsStrokeLuminanceMessageShown) {
                appSettings.SettingsStrokeLuminanceMessageShown = true;
                let lumMessage = `||SettingsStrokeLuminanceMessage||`;
                alertShowFooter('luminancemessage', 'info', lumMessage, true);
            }
        }
        
        if(regions && window.currentSection.CurrentApplication.RegionType !== RegionType_None) {
            //Draw region boxes
            ctx.globalAlpha = 0.1;
            ctx.lineWidth = 2;
            ctx.strokeStyle = borderColor;
    
            //Determine grid
            let cols = 0;
            let rows = 0;
    
            switch(window.currentSection.CurrentApplication.RegionType) {
                case RegionType_VerticalSplit:
                    cols = 2;
                    rows = 1;
                    break;
                case RegionType_HorizontalSplit:
                    cols = 1;
                    rows = 2;
                    break;
                case RegionType_Quadrant:
                    cols = 2;
                    rows = 2;
                    break;
                case RegionType_Grid:
                    cols = 3;
                    rows = 3;
                    break;                                                
                case RegionType_Custom:
                    cols = window.currentSection.CurrentApplication.RegionCustomCols;
                    rows = window.currentSection.CurrentApplication.RegionCustomRows;
                    break;
            }
    
            if(cols > 0 && rows > 0) {
                let cellWidth = (width - ctx.lineWidth) / cols;
                let cellHeight = (height - ctx.lineWidth) / rows;
                ctx.lineCap = 'butt';
                ctx.fillStyle = regionColor;
    
                ctx.beginPath();
                ctx.rect(0, 0, width, height);
                for(let g = 0; g < regions.length; g++) {
                    ctx.rect((ctx.lineWidth / 2)+(regions[g].RegionColumn * cellWidth) - cellWidth, 
                             (ctx.lineWidth / 2)+(regions[g].RegionRow * cellHeight) - cellHeight, 
                             cellWidth,
                             cellHeight);      
                    ctx.fillRect((ctx.lineWidth)+(regions[g].RegionColumn * cellWidth) - cellWidth, 
                                 (ctx.lineWidth)+(regions[g].RegionRow * cellHeight) - cellHeight, 
                                 cellWidth - ctx.lineWidth,
                                 cellHeight - ctx.lineWidth);                                     
                }
                ctx.stroke();
            }
        } else {
            //Draw border around canvas
            ctx.globalAlpha = 0.2;
            ctx.lineWidth = 1;
            ctx.strokeStyle = borderColor;
    
            ctx.beginPath();
            ctx.rect(0, 0, width, height);
            ctx.stroke();
        } 
    
        if(gesture && gesture.PointPatterns.length > 0) {
    
            //Position adjustment
            let shiftX = Math.ceil(penWidth / width) * (penWidth + 1);
            let shiftY = Math.ceil(penWidth / height) * (penWidth + 1);
    
            //Bounds adjustment
            let adjustedWidth = width - (shiftX * 4);
            let adjustedHeight = height - (shiftY * 4);
    
            let scaledGesture = this.Scale(gesture.PointPatterns[0].Points, adjustedWidth, adjustedHeight);
    
            //Center gesture
            shiftX = Math.floor(((width / 2) - (scaledGesture.ScaledWidth / 2)));
            shiftY = Math.floor(((height / 2) - (scaledGesture.ScaledHeight / 2)));   
    
            /*
            //Circle start cap
            ctx.beginPath();
            ctx.arc(scaledGesture.ScaledStroke[0].X + shiftX, 
                scaledGesture.ScaledStroke[0].Y + shiftY, 
                penWidth * 0.5, 
                0, 
                2 * Math.PI);
            ctx.stroke();
            */
    
            //Stroke line to give border first
            ctx.globalAlpha = 1.0;
            ctx.lineWidth = penWidth + 2;
            ctx.strokeStyle = penBorder;    
            ctx.beginPath();
            ctx.lineCap = 'round';
    
            //Create actual gesture path
            for (let i = 0; i < scaledGesture.ScaledStroke.length - 1; i++) {
                ctx.moveTo(scaledGesture.ScaledStroke[i].X + shiftX, 
                    scaledGesture.ScaledStroke[i].Y + shiftY);
                ctx.lineTo(scaledGesture.ScaledStroke[i+1].X + shiftX, 
                    scaledGesture.ScaledStroke[i+1].Y + shiftY);
            }        
    
            //Draw border line first
            if(drawBorder) {
                ctx.stroke(); 
            }  
    
            //Draw actual gesture line on top of the border line stroke
            //Uses same path as above, just changes stroke properties and draws again
            //ctx.globalAlpha = 0.7;
            ctx.lineWidth = penWidth;
            ctx.strokeStyle = penColor;
            ctx.stroke();   
    
            if(drawBorder) {
                //Draw arrow end cap border
                ctx.globalAlpha = 1.0;
                this.DrawArrowBorder(ctx, 
                    scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-2].X + shiftX,
                    scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-2].Y + shiftY,
                    scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-1].X + shiftX,
                    scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-1].Y + shiftY,
                    penWidth * 2,
                    penBorder);        
            }
    
            //Draw arrow end cap
            ctx.globalAlpha = 1.0;
            this.DrawArrowTip(ctx, 
                scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-2].X + shiftX,
                scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-2].Y + shiftY,
                scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-1].X + shiftX,
                scaledGesture.ScaledStroke[scaledGesture.ScaledStroke.length-1].Y + shiftY,
                penWidth * 2,
                arrowColor);        
    
        } else {
            ctx.globalAlpha = 0.5;
            ctx.textBaseline = 'top';
            ctx.textAlign = 'center';
            let fontSize = width;
            let fontName = 'px -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,"Noto Sans",sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji"';
            ctx.font = fontSize + fontName;
            ctx.fillStyle = borderColor;
            var noGestureText = `||ActionDefaultGestureLinkText||`;
            while(ctx.measureText(noGestureText).width > width * 0.8) {
                fontSize--;
                ctx.font = fontSize + fontName;
            }
            let textHeight = stringTextHeight(ctx.font);
            ctx.fillText(noGestureText, parseInt(width / 2), parseInt(height / 2) - parseInt(textHeight.ascent / 2), width);        
        }
    }

    Scale(points, width, height) {
        // Create generic list of points to hold scaled stroke
        let ScaledStroke = [];
    
        // Get total width and height of gesture
        let fGestureOffsetLeft = points.reduce(function(prev, curr) {
            return prev.X < curr.X ? prev : curr;
        }).X;
        let fGestureOffsetTop = points.reduce(function(prev, curr) {
            return prev.Y < curr.Y ? prev : curr;
        }).Y;
        let fGestureWidth = points.reduce(function(prev, curr) {
            return prev.X > curr.X ? prev : curr;
        }).X - fGestureOffsetLeft;
        let fGestureHeight = points.reduce(function(prev, curr) {
            return prev.Y > curr.Y ? prev : curr;
        }).Y - fGestureOffsetTop;
    
        // Get each scale ratio
        let dScaleX = width / fGestureWidth;
        let dScaleY = height / fGestureHeight;
    
        let ScaledWidth = 0;
        let ScaledHeight = 0;
    
        // Scale on the longest axis
        if (fGestureWidth >= fGestureHeight)
        {
            // Scale on X axis
    
            for (let i = 0; i < points.length; i++) {
                ScaledStroke.push({ X: ((points[i].X - fGestureOffsetLeft) * dScaleX), Y: ((points[i].Y - fGestureOffsetTop) * dScaleX)});
            }
    
            // Calculate new gesture width and height
            ScaledWidth = Math.floor(fGestureWidth * dScaleX);
            ScaledHeight = Math.floor(fGestureHeight * dScaleX)
        }
        else
        {
            // Scale on Y axis
    
            for (let i = 0; i < points.length; i++) {
                ScaledStroke.push({ X: ((points[i].X - fGestureOffsetLeft) * dScaleY), Y: ((points[i].Y - fGestureOffsetTop) * dScaleY)});
            }
    
            // Calculate new gesture width and height
            ScaledWidth = Math.floor(fGestureWidth * dScaleY);
            ScaledHeight = Math.floor(fGestureHeight * dScaleY)        
        }
    
        return { ScaledWidth, ScaledHeight, ScaledStroke };
    }

    SelectModalPostRender() {
        $('#modalStandardCenter canvas').each(function( index, value ) {
            gestures.DrawToCanvas(appSettings.Gestures.find(g => g.Name === $(value).data('gesturename')), 
                value,
                null,
                colorNetToHex($(value).data('usesecondary') ? appSettings.SecondaryPenColor : appSettings.PenColor)
            ); 
        });   
        if(gestures.NewGestureName.length > 0) {
            modalDismissStandardCenter();
            window.currentSection.ActionList.ActionEditor.UpdateGesture(gestures.NewGestureName);
        }
    }

    SelectModalBodyHTML() {

        let bodyHTML = "<div class='row justify-content-center gestureSelectModal'>";
    
        var sortedGestures = appSettings.Gestures.sort((a, b) => a.Name.localeCompare(b.Name, undefined, { sensitivity: 'accent' }));
        
        $.each(sortedGestures, function(index, item) {
            bodyHTML += `<div class="d-flex">
                            <figure class="figure">
                                <canvas class="d-block pt-1 px-2 cursor-pointer"
                                        data-regions=""
                                        data-gesturename="${stringEscapeProperty(item.Name)}" 
                                        data-usesecondary="${currentSection.ActionList.CurrentAction.Action.UseSecondaryStrokeButton}"
                                        width="125"
                                        height="125" 
                                        onclick="window.currentSection.ActionList.ActionEditor.UpdateGesture(\`${stringEscapeProperty(item.Name)}\`);">
                                </canvas>
                                <figcaption class="figure-caption text-center">${stringEscapeHtml(item.Name)}</figcaption>
                            </figure>
                         </div>`
        });
    
        bodyHTML += "</div>";
        
        return bodyHTML;
    }

    SelectModalNewGesture() {
        this.NewGestureName = '';
        hostPostMessage('NewGesture', '', appSettings);
    }

    SelectModalNoGesture() {
        modalDismissStandardCenter();
        window.currentSection.ActionList.ActionEditor.UpdateGesture("");
    }

    SelectModalRefreshBody() {
        $('#modalStandardCenter .modal-body').html(this.SelectModalBodyHTML());
        this.SelectModalPostRender();
    }

    ShowSelectModal() {

        $('#modalStandardCenter').find('.modal-dialog').addClass('modal-dialog-xxl');
        modalStandardCenterTitle = stringEscapeHtml(`||frmSelectGestureTitle||`);
        modalStandardShowCallback = "gestures.SelectModalPostRender()";
        modalStandardCenterBody = this.SelectModalBodyHTML();
        modalStandardCenterButtons = "nogesture|gestures.SelectModalNoGesture(),newgesture|gestures.SelectModalNewGesture(),cancel";
    
        $('#modalStandardCenter').modal('show');
    }

    UpdateGestures(gesturesNode) {
        appSettings.Gestures = gesturesNode;
    }
}


/********************************************************************************************
    GestureList - Manage Gestures
*********************************************************************************************/

class GestureList {
    constructor() {

    }

    get HTML() {
        return `Gesture List HTML`;
    }

    LoadGestures() {

    }
}