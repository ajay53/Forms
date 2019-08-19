﻿using Android.Views;
using CrossWordHelper;
using Forms.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Forms.Droid.Effects
{
    class CrossWordButtonEffect : PlatformEffect
    {
        Android.Views.View view;
        Element formsElement;
        TouchEffect libTouchEffect;
        bool capture;
        Func<double, double> fromPixels;
        int[] twoIntArray = new int[2];

        string wordResult = string.Empty;

        static Dictionary<Android.Views.View, CrossWordButtonEffect> viewDictionary =
            new Dictionary<Android.Views.View, CrossWordButtonEffect>();

        static Dictionary<int, CrossWordButtonEffect> idToEffectDictionary =
            new Dictionary<int, CrossWordButtonEffect>();

        GridButton gridButton = new GridButton();

        protected override void OnAttached()
        {
            // Get the Android View corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            TouchEffect touchEffect =
                (TouchEffect)Element.Effects.
                    FirstOrDefault(e => e is TouchEffect);

            if (touchEffect != null && view != null)
            {
                viewDictionary.Add(view, this);

                formsElement = Element;

                libTouchEffect = touchEffect;

                // Save fromPixels function
                fromPixels = view.Context.FromPixels;

                // Set event handler on View
                view.Touch += OnTouch;
            }
        }

        protected override void OnDetached()
        {
            if (viewDictionary.ContainsKey(view))
            {
                viewDictionary.Remove(view);
                view.Touch -= OnTouch;
            }
        }

        private void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
        {
            // Two object common to all the events
            Android.Views.View senderView = sender as Android.Views.View;
            MotionEvent motionEvent = args.Event;
            // Get the pointer index
            int pointerIndex = motionEvent.ActionIndex;

            // Get the id that identifies a finger over the course of its progress
            int id = motionEvent.GetPointerId(pointerIndex);

            senderView.GetLocationOnScreen(twoIntArray);
            Point screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                  twoIntArray[1] + motionEvent.GetY(pointerIndex));

            // Use ActionMasked here rather than Action to reduce the number of possibilities
            switch (args.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    Console.WriteLine("GridBuuttonRenderer");

                    //CrossWordDrawLineViewModel.touchPressed = new SKPoint(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                    //twoIntArray[1] + motionEvent.GetY(pointerIndex));

                    CrossWordDrawLineViewModel.changeLineColor = false;
                    CrossWordDrawLineViewModel.changeLineColorMethod();
                    CrossWordDrawLineViewModel.touchPressed = GetPressedViewCenterPoint(senderView);

                    FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);

                    idToEffectDictionary.Add(id, this);

                    capture = libTouchEffect.Capture;
                    break;

                case MotionEventActions.Move:
                    // Multiple Move events are bundled, so handle them in a loop
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);

                        if (capture)
                        {

                            senderView.GetLocationOnScreen(twoIntArray);


                            screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                            twoIntArray[1] + motionEvent.GetY(pointerIndex));

                            FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                        }
                        else
                        {
                            CrossWordDrawLineViewModel.changeLineColor = false;
                            CheckForBoundaryHop(id, screenPointerCoords);
                            new MessageCenterHelper().SendMessageMenu(null);

                            if (idToEffectDictionary[id] != null)
                            {
                                FireEvent(idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
                            }
                        }
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    }
                    else
                    {
                        CheckForBoundaryHop(id, screenPointerCoords);
                        //CrossWordDrawLineViewModel.touchReleased = new SKPoint((float)screenPointerCoords.X, (float)screenPointerCoords.Y);

                        CrossWordDrawLineViewModel.changeLineColor = false;
                        new MessageCenterHelper().SendMessageMenu(null);

                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;

                case MotionEventActions.Cancel:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    }
                    else
                    {
                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;
            }
        }

        void CheckForBoundaryHop(int id, Point pointerLocation)
        {
            CrossWordButtonEffect touchEffectHit = null;

            foreach (Android.Views.View view in viewDictionary.Keys)
            {
                // Get the view rectangle
                try
                {
                    view.GetLocationOnScreen(twoIntArray);
                }
                catch // System.ObjectDisposedException: Cannot access a disposed object.
                {
                    continue;
                }
                Rectangle viewRect = new Rectangle(twoIntArray[0], twoIntArray[1], view.Width, view.Height);

                if (viewRect.Contains(pointerLocation))
                {
                    CrossWordDrawLineViewModel.touchReleased = GetReleasedViewCenterPoint(view);
                    touchEffectHit = viewDictionary[view];
                }
            }

            if (touchEffectHit != idToEffectDictionary[id])
            {
                if (idToEffectDictionary[id] != null)
                {
                    FireEvent(idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);
                }
                if (touchEffectHit != null)
                {
                    FireEvent(touchEffectHit, id, TouchActionType.Entered, pointerLocation, true);
                }
                idToEffectDictionary[id] = touchEffectHit;
            }
        }

        void FireEvent(CrossWordButtonEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            // Get the method to call for firing events
            Action<Element, TouchActionEventArgs> onTouchAction = touchEffect.libTouchEffect.OnTouchAction;

            // Get the location of the pointer within the view
            touchEffect.view.GetLocationOnScreen(twoIntArray);
            double x = pointerLocation.X - twoIntArray[0];
            double y = pointerLocation.Y - twoIntArray[1];
            Point point = new Point(fromPixels(x), fromPixels(y));

            // Call the method
            onTouchAction(touchEffect.formsElement,
                new TouchActionEventArgs(id, actionType, point, isInContact));
        }

        SKPoint GetPressedViewCenterPoint(Android.Views.View senderView)
        {

            var centerX = senderView.GetX() + senderView.Width / 2;
            var centerY = senderView.GetY() + senderView.Height / 2;

            SKPoint viewCenter = new SKPoint(centerX, centerY);

            return viewCenter;
        }

        SKPoint GetReleasedViewCenterPoint(Android.Views.View senderView)
        {

            var centerX = senderView.GetX() + senderView.Width / 2;
            var centerY = senderView.GetY() + senderView.Height / 2;

            SKPoint viewCenter = new SKPoint(centerX, centerY);

            return viewCenter;
        }
    }
}