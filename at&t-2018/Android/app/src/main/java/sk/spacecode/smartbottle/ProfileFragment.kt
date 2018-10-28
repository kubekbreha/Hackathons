package sk.spacecode.smartbottle

import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import com.google.firebase.database.*


class ProfileFragment : Fragment() {

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        var view = inflater.inflate(R.layout.fragment_profile, container, false)
        var viewName = view.findViewById<EditText>(R.id.fragment_profile_name)
        var viewEmail = view.findViewById<EditText>(R.id.fragment_profile_email)
        var viewWeight = view.findViewById<EditText>(R.id.fragment_profile_weight)

        viewName.maxLines = 1
        viewEmail.maxLines = 1
        viewWeight.maxLines = 1

        viewEmail.setText("spacecode@gmail.com")

        getWeightFromDatabase(viewName)
        getUserNameFromDatabase(viewWeight)

        return view
    }


    fun getWeightFromDatabase(view: EditText){
        var mDatabase: DatabaseReference?
        mDatabase = FirebaseDatabase.getInstance().reference
        val rootRef = mDatabase!!.child("20:13:10:17:10:29").child("personalData").child("weight")

        rootRef.addListenerForSingleValueEvent(object : ValueEventListener {
            override fun onCancelled(p0: DatabaseError) {}

            override fun onDataChange(p0: DataSnapshot) {
                MainActivity.userWeight = p0.value.toString()
                view.setText( p0.value.toString())

            }
        })
    }


    fun getUserNameFromDatabase(view: EditText){
        var mDatabase: DatabaseReference?
        mDatabase = FirebaseDatabase.getInstance().reference
        val rootRef = mDatabase!!.child("20:13:10:17:10:29").child("personalData").child("name")

        rootRef.addListenerForSingleValueEvent(object : ValueEventListener {
            override fun onCancelled(p0: DatabaseError) {}

            override fun onDataChange(p0: DataSnapshot) {
                MainActivity.userName = p0.value.toString()
                view.setText( p0.value.toString())
            }
        })
    }
}
