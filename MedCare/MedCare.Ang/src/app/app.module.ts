import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes, Router, CanActivate } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlashMessagesModule, FlashMessagesService } from 'angular2-flash-messages';

import { AuthService } from './services/auth.service';
import { DoctorService } from './services/doctor.service';
import { PatientService } from './services/patient.service';
import { MessageService } from './services/message.service';
import { AdminGuard } from './guards/adminGuard';
import { DocGuard } from './guards/docGuard';
import { PatGuard } from './guards/patGuard';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RecordsComponent } from './components/records/records.component';
import { MessagesComponent } from './components/messages/messages.component';
import { ListDoctorsComponent } from './components/list-doctors/list-doctors.component';
import { DoctorDetailComponent } from './components/doctor-detail/doctor-detail.component';
import { AddDoctorComponent } from './components/add-doctor/add-doctor.component';
import { EditDoctorComponent } from './components/edit-doctor/edit-doctor.component';
import { PatientRecordsComponent } from './components/patient-records/patient-records.component';
import { AddPatientComponent } from './components/add-patient/add-patient.component';
import { RecordDetailComponent } from './components/record-detail/record-detail.component';
import { AddRecordComponent } from './components/add-record/add-record.component';
import { RecordDetail2Component } from './components/record-detail2/record-detail2.component';
import { ExistingPatientComponent } from './components/existing-patient/existing-patient.component';
import { PatientsComponent } from './components/patients/patients.component';
import { PatientDetailComponent } from './components/patient-detail/patient-detail.component';
import { MessageComponent } from './components/message/message.component';
import { SendmessageComponent } from './components/sendmessage/sendmessage.component';
import { Sendmessage2Component } from './components/sendmessage2/sendmessage2.component';
import { DoctorsComponent } from './components/doctors/doctors.component';


const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'records', component: RecordsComponent, canActivate: [DocGuard] },
  { path: 'record-detail2/:id', component: RecordDetail2Component, canActivate: [DocGuard] },
  { path: 'edit-doctor', component: EditDoctorComponent, canActivate: [DocGuard] },
  { path: 'add-patient', component: AddPatientComponent, canActivate: [DocGuard] },
  { path: 'existing-patient', component: ExistingPatientComponent, canActivate: [DocGuard] },
  { path: 'patients', component: PatientsComponent, canActivate: [DocGuard] },
  { path: 'patient-detail/:id', component: PatientDetailComponent, canActivate: [DocGuard] },
  { path: 'patient-records', component: PatientRecordsComponent, canActivate: [PatGuard] },
  { path: 'add-record', component: AddRecordComponent, canActivate: [PatGuard] },
  { path: 'record-detail/:id', component: RecordDetailComponent, canActivate: [PatGuard] },
  { path: 'doctors', component: DoctorsComponent, canActivate: [PatGuard] },
  { path: 'messages', component: MessagesComponent },
  { path: 'sendmessage', component: SendmessageComponent, canActivate: [PatGuard] },
  { path: 'sendmessage2', component: Sendmessage2Component, canActivate: [DocGuard] },
  { path: 'message/:id', component: MessageComponent },
  { path: 'list-doctors', component: ListDoctorsComponent, canActivate: [AdminGuard] },
  { path: 'add-doctor', component: AddDoctorComponent, canActivate: [AdminGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'doctor-detail/:id', component: DoctorDetailComponent },
  { path: 'navbar', component: NavbarComponent },
  { path: 'edit-doctor/:id', component: EditDoctorComponent }
]

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    RecordsComponent,
    MessagesComponent,
    HomeComponent,
    LoginComponent,
    ListDoctorsComponent,
    DoctorDetailComponent,
    AddDoctorComponent,
    EditDoctorComponent,
    PatientRecordsComponent,
    AddPatientComponent,
    RecordDetailComponent,
    AddRecordComponent,
    RecordDetail2Component,
    ExistingPatientComponent,
    PatientsComponent,
    PatientDetailComponent,
    MessageComponent,
    SendmessageComponent,
    Sendmessage2Component,
    DoctorsComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    RouterModule.forRoot(appRoutes),
    FormsModule,
    ReactiveFormsModule,
    FlashMessagesModule.forRoot()
  ],
  providers: [
    AuthService,
    DoctorService,
    PatientService,
    MessageService,
    AdminGuard,
    DocGuard,
    PatGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
