/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/define-route');

export interface IDefineRoute {
    applicationId?: string;
    environmentId?: string;
    ingressId?: string;
    path?: string;
    targetMicroservice?: string;
    targetPath?: string;
}

export class DefineRouteValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        ingressId: new Validator(),
        path: new Validator(),
        targetMicroservice: new Validator(),
        targetPath: new Validator(),
    };
}

export class DefineRoute extends Command<IDefineRoute> implements IDefineRoute {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/define-route';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new DefineRouteValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _ingressId!: string;
    private _path!: string;
    private _targetMicroservice!: string;
    private _targetPath!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
            'path',
            'targetMicroservice',
            'targetPath',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get ingressId(): string {
        return this._ingressId;
    }

    set ingressId(value: string) {
        this._ingressId = value;
        this.propertyChanged('ingressId');
    }
    get path(): string {
        return this._path;
    }

    set path(value: string) {
        this._path = value;
        this.propertyChanged('path');
    }
    get targetMicroservice(): string {
        return this._targetMicroservice;
    }

    set targetMicroservice(value: string) {
        this._targetMicroservice = value;
        this.propertyChanged('targetMicroservice');
    }
    get targetPath(): string {
        return this._targetPath;
    }

    set targetPath(value: string) {
        this._targetPath = value;
        this.propertyChanged('targetPath');
    }

    static use(initialValues?: IDefineRoute): [DefineRoute, SetCommandValues<IDefineRoute>] {
        return useCommand<DefineRoute, IDefineRoute>(DefineRoute, initialValues);
    }
}
