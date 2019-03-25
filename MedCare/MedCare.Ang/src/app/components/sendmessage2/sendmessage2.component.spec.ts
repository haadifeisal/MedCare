import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Sendmessage2Component } from './sendmessage2.component';

describe('Sendmessage2Component', () => {
  let component: Sendmessage2Component;
  let fixture: ComponentFixture<Sendmessage2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Sendmessage2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Sendmessage2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
