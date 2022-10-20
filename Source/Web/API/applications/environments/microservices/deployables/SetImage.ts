/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environmentId}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image');

export interface ISetImage {
    microserviceId?: string;
    deployableId?: string;
    deployableImageName?: string;
}

export class SetImageValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        microserviceId: new Validator(),
        deployableId: new Validator(),
        deployableImageName: new Validator(),
    };
}

export class SetImage extends Command<ISetImage> implements ISetImage {
    readonly route: string = '/api/applications/{applicationId}/environments/{environmentId}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetImageValidator();

    private _microserviceId!: string;
    private _deployableId!: string;
    private _deployableImageName!: string;

    get requestArguments(): string[] {
        return [
            'microserviceId',
            'deployableId',
        ];
    }

    get properties(): string[] {
        return [
            'microserviceId',
            'deployableId',
            'deployableImageName',
        ];
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

    static use(initialValues?: ISetImage): [SetImage, SetCommandValues<ISetImage>] {
        return useCommand<SetImage, ISetImage>(SetImage, initialValues);
    }
}
