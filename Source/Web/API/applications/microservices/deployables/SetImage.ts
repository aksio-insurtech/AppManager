/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image');

export interface ISetImage {
    applicationId?: string;
    microserviceId?: string;
    deployableId?: string;
    deployableImageName?: string;
}

export class SetImageValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        microserviceId: new Validator(),
        deployableId: new Validator(),
        deployableImageName: new Validator(),
    };
}

export class SetImage extends Command<ISetImage> implements ISetImage {
    readonly route: string = '/api/applications/{{applicationId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetImageValidator();

    private _applicationId!: string;
    private _microserviceId!: string;
    private _deployableId!: string;
    private _deployableImageName!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'microserviceId',
            'deployableId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'microserviceId',
            'deployableId',
            'deployableImageName',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get deployableId(): string {
        return this._deployableId;
    }

    set deployableId(value: string) {
        this._deployableId = value;
        this.propertyChanged('deployableId');
    }
    get deployableImageName(): string {
        return this._deployableImageName;
    }

    set deployableImageName(value: string) {
        this._deployableImageName = value;
        this.propertyChanged('deployableImageName');
    }

    static use(initialValues?: ISetImage): [SetImage, SetCommandValues<ISetImage>, ClearCommandValues] {
        return useCommand<SetImage, ISetImage>(SetImage, initialValues);
    }
}
